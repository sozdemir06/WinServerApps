using Accounting.Taxes.Dtos;
using Accounting.Taxes.Exceptions;
using Accounting.Taxes.Models;

namespace Accounting.Taxes.Features.TenantTaxes.UpdateTenantTax;

public record UpdateTenantTaxCommand(Guid Id, TenantTaxDto TenantTax) : ICommand<UpdateTenantTaxResult>, ICacheRemovingRequest
{
  public List<string> CacheKeysToRemove => [CacheKeys.TenantTaxes];
}

public record UpdateTenantTaxResult(Guid Id);

public class UpdateTenantTaxCommandValidator : AbstractValidator<UpdateTenantTaxCommand>
{
  public UpdateTenantTaxCommandValidator()
  {
    RuleFor(x => x.Id)
        .NotEmpty()
        .WithMessage("TenantTax ID is required.");

    RuleFor(x => x.TenantTax)
        .NotNull()
        .WithMessage("TenantTax data is required.");

    RuleFor(x => x.TenantTax.Rate)
        .InclusiveBetween(0, 100)
        .WithMessage("Tax rate must be between 0 and 100.");

    RuleFor(x => x.TenantTax.Translates)
        .NotEmpty()
        .WithMessage("At least one translation is required.");

    RuleFor(x => x.TenantTax.TenantId)
        .NotNull()
        .WithMessage("TenantId is required.");
  }
}

public class UpdateTenantTaxHandler : ICommandHandler<UpdateTenantTaxCommand, UpdateTenantTaxResult>
{
  private readonly AccountingDbContext _context;

  public UpdateTenantTaxHandler(AccountingDbContext context)
  {
    _context = context;
  }

  public async Task<UpdateTenantTaxResult> Handle(UpdateTenantTaxCommand request, CancellationToken cancellationToken)
  {
    var tenantTax = await _context.TenantTaxes
        .Include(tt => tt.TenantTaxTranslates)
        .FirstOrDefaultAsync(tt => tt.Id == request.Id, cancellationToken);

    if (tenantTax == null)
    {
      throw new TaxNotFoundException(request.Id);
    }

    if (request.TenantTax.Translates.Count == 0)
    {
      throw new TaxBadRequestException("At least one translation is required.");
    }

    // If the TenantTax is being set as default, unset any existing default TenantTax for this tenant
    if (request.TenantTax.IsDefault && !tenantTax.IsDefault)
    {
      var existingDefault = await _context.TenantTaxes
          .Where(tt => tt.IsDefault && !tt.IsDeleted && tt.Id != request.Id && tt.TenantId == request.TenantTax.TenantId)
          .FirstOrDefaultAsync(cancellationToken);

      if (existingDefault != null)
      {
        existingDefault.UnsetAsDefault();
      }
    }

    // Update TenantTax properties
    tenantTax.Update(request.TenantTax.Rate, request.TenantTax.IsActive);

    if (request.TenantTax.IsDefault && !tenantTax.IsDefault)
    {
      tenantTax.SetAsDefault();
    }
    else if (!request.TenantTax.IsDefault && tenantTax.IsDefault)
    {
      tenantTax.UnsetAsDefault();
    }

    // Validate translations
    var validTranslations = request.TenantTax.Translates
        .Where(t => !string.IsNullOrWhiteSpace(t.Name) && t.LanguageId.HasValue)
        .ToList();

    // Check for existing TenantTaxes with the same translation names for this tenant (excluding current)
    var existingTenantTaxNames = validTranslations.Select(t => t.Name).ToList();

    var existingTenantTaxes = await _context.TenantTaxes
        .Where(tt => tt.Id != request.Id && tt.TenantId == request.TenantTax.TenantId &&
                    tt.TenantTaxTranslates.Any(ttt => existingTenantTaxNames.Contains(ttt.Name)))
        .AsNoTracking()
        .ToListAsync(cancellationToken);

    if (existingTenantTaxes.Any())
    {
      throw new TaxBadRequestException("A TenantTax with one of the provided names already exists for this tenant.");
    }

    // Clear existing translations
    _context.TenantTaxTranslates.RemoveRange(tenantTax.TenantTaxTranslates);

    // Add new translations
    foreach (var transDto in validTranslations)
    {
      var newTranslate = TenantTaxTranslate.Create(transDto.Name, transDto.Description, transDto.LanguageId, tenantTax.Id);
      _context.TenantTaxTranslates.Add(newTranslate);
    }

    await _context.SaveChangesAsync(cancellationToken);

    return new UpdateTenantTaxResult(tenantTax.Id);
  }
}