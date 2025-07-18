using Accounting.Taxes.Dtos;
using Accounting.Taxes.Exceptions;
using Accounting.Taxes.Models;
using Accounting.TaxGroups.Exceptions;

namespace Accounting.Taxes.Features.TenantTaxes.AddTenantTaxToTenantTaxGroup;

public record AddTenantTaxToTenantTaxGroupCommand(Guid TenantTaxGroupId, TenantTaxDto TenantTax,Guid? tenantId) : ICommand<AddTenantTaxToTenantTaxGroupResult>, ICacheRemovingRequest
{
  public List<string> CacheKeysToRemove => [CacheKeys.TenantTaxes, CacheKeys.TenantTaxGroups];
}

public record AddTenantTaxToTenantTaxGroupResult(Guid TenantTaxId, Guid TenantTaxGroupId);

public class AddTenantTaxToTenantTaxGroupCommandValidator : AbstractValidator<AddTenantTaxToTenantTaxGroupCommand>
{
  public AddTenantTaxToTenantTaxGroupCommandValidator()
  {
    RuleFor(x => x.TenantTaxGroupId)
        .NotEmpty()
        .WithMessage("TenantTaxGroup ID is required.");

    RuleFor(x => x.TenantTax)
        .NotNull()
        .WithMessage("TenantTax data is required.");

    RuleFor(x => x.TenantTax.Rate)
        .InclusiveBetween(0, 100)
        .WithMessage("Tax rate must be between 0 and 100.");

    RuleFor(x => x.TenantTax.Translates)
        .NotEmpty()
        .WithMessage("At least one translation is required.");

    RuleFor(x => x.tenantId)
        .NotNull()
        .WithMessage("TenantId is required.");
  }
}

public class AddTenantTaxToTenantTaxGroupHandler : ICommandHandler<AddTenantTaxToTenantTaxGroupCommand, AddTenantTaxToTenantTaxGroupResult>
{
  private readonly AccountingDbContext _context;

  public AddTenantTaxToTenantTaxGroupHandler(AccountingDbContext context)
  {
    _context = context;
  }

  public async Task<AddTenantTaxToTenantTaxGroupResult> Handle(AddTenantTaxToTenantTaxGroupCommand request, CancellationToken cancellationToken)
  {
    // Verify the tenant tax group exists
    var tenantTaxGroup = await _context.TenantTaxGroups
        .FirstOrDefaultAsync(ttg => ttg.Id == request.TenantTaxGroupId, cancellationToken);

    if (tenantTaxGroup == null)
    {
      throw new TaxGroupNotFoundException(request.TenantTaxGroupId);
    }

    if (request.TenantTax.Translates.Count == 0)
    {
      throw new TaxBadRequestException("At least one translation is required.");
    }

    // If the new TenantTax is being set as default, unset any existing default TenantTax in this tenant tax group
    if (request.TenantTax.IsDefault)
    {
      var existingDefault = await _context.TenantTaxes
          .Where(tt => tt.TenantTaxGroupId == request.TenantTaxGroupId && tt.IsDefault)
          .FirstOrDefaultAsync(cancellationToken);

      if (existingDefault != null)
      {
        existingDefault.UnsetAsDefault();
      }
    }

    var tenantTax = TenantTax.Create(request.TenantTax.Rate, request.TenantTax.IsActive, request.tenantId);
    tenantTax.UpdateTenantTaxGroup(request.TenantTaxGroupId);

    // Set as default if requested
    if (request.TenantTax.IsDefault)
    {
      tenantTax.SetAsDefault();
    }

    // Validate translations
    var validTranslations = request.TenantTax.Translates
        .Where(t => !string.IsNullOrWhiteSpace(t.Name) && t.LanguageId.HasValue)
        .ToList();

    // Check for existing TenantTaxes with the same translation names in this tenant tax group
    var existingTenantTaxNames = validTranslations.Select(t => t.Name).ToList();

    var existingTenantTaxes = await _context.TenantTaxes
        .Where(tt => tt.TenantTaxGroupId == request.TenantTaxGroupId &&
                    tt.TenantTaxTranslates.Any(ttt => existingTenantTaxNames.Contains(ttt.Name)))
        .AsNoTracking()
        .ToListAsync(cancellationToken);

    if (existingTenantTaxes.Any())
    {
      throw new TaxBadRequestException("A TenantTax with one of the provided names already exists in this tenant tax group.");
    }

    await _context.TenantTaxes.AddAsync(tenantTax, cancellationToken);

    // Add translations
    foreach (var transDto in validTranslations)
    {
      var newTranslate = TenantTaxTranslate.Create(transDto.Name, transDto.Description, transDto.LanguageId, tenantTax.Id);
      _context.TenantTaxTranslates.Add(newTranslate);
    }

    await _context.SaveChangesAsync(cancellationToken);

    return new AddTenantTaxToTenantTaxGroupResult(tenantTax.Id, request.TenantTaxGroupId);
  }
}