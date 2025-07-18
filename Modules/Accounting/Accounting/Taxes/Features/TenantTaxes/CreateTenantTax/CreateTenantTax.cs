using Accounting.Taxes.Dtos;
using Accounting.Taxes.Exceptions;
using Accounting.Taxes.Models;

namespace Accounting.Taxes.Features.TenantTaxes.CreateTenantTax;

public record CreateTenantTaxCommand(TenantTaxDto TenantTax,Guid? tenantId) : ICommand<CreateTenantTaxResult>, ICacheRemovingRequest
{
  public List<string> CacheKeysToRemove => [CacheKeys.TenantTaxes];
}

public record CreateTenantTaxResult(Guid Id);

public class CreateTenantTaxCommandValidator : AbstractValidator<CreateTenantTaxCommand>
{
  public CreateTenantTaxCommandValidator()
  {
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

public class CreateTenantTaxHandler : ICommandHandler<CreateTenantTaxCommand, CreateTenantTaxResult>
{
  private readonly AccountingDbContext _context;

  public CreateTenantTaxHandler(AccountingDbContext context)
  {
    _context = context;
  }

  public async Task<CreateTenantTaxResult> Handle(CreateTenantTaxCommand request, CancellationToken cancellationToken)
  {
    if (request.TenantTax.Translates.Count == 0)
    {
      throw new TaxBadRequestException("At least one translation is required.");
    }

    // If the new TenantTax is being set as default, unset any existing default TenantTax for this tenant
    if (request.TenantTax.IsDefault)
    {
      var existingDefault = await _context.TenantTaxes
          .Where(tt => tt.IsDefault && tt.TenantId == request.TenantTax.TenantId)
          .FirstOrDefaultAsync(cancellationToken);

      if (existingDefault != null)
      {
        existingDefault.UnsetAsDefault();
      }
    }

    var tenantTax = TenantTax.Create(request.TenantTax.Rate, request.TenantTax.IsActive, request.tenantId);

    // Set as default if requested
    if (request.TenantTax.IsDefault)
    {
      tenantTax.SetAsDefault();
    }

    // Validate translations
    var validTranslations = request.TenantTax.Translates
        .Where(t => !string.IsNullOrWhiteSpace(t.Name) && t.LanguageId.HasValue)
        .ToList();

    // Check for existing TenantTaxes with the same translation names for this tenant
    var existingTenantTaxNames = validTranslations.Select(t => t.Name).ToList();

    var existingTenantTaxes = await _context.TenantTaxes
        .Where(tt => tt.TenantTaxTranslates.Any(ttt => existingTenantTaxNames.Contains(ttt.Name)))
        .AsNoTracking()
        .ToListAsync(cancellationToken);

    if (existingTenantTaxes.Any())
    {
      throw new TaxBadRequestException("A TenantTax with one of the provided names already exists for this tenant.");
    }

    await _context.TenantTaxes.AddAsync(tenantTax, cancellationToken);

    // Add translations
    foreach (var transDto in validTranslations)
    {
      var newTranslate = TenantTaxTranslate.Create(transDto.Name, transDto.Description, transDto.LanguageId, tenantTax.Id);
      _context.TenantTaxTranslates.Add(newTranslate);
    }

    await _context.SaveChangesAsync(cancellationToken);

    return new CreateTenantTaxResult(tenantTax.Id);
  }
}