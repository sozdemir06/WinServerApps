using Accounting.Taxes.Dtos;
using Accounting.Taxes.Exceptions;
using Accounting.Taxes.Models;

namespace Accounting.Taxes.Features.AdminTaxes.CreateAdminTax;

public record CreateAdminTaxCommand(TaxDto Tax) : ICommand<CreateAdminTaxResult>, ICacheRemovingRequest
{
  public List<string> CacheKeysToRemove => [CacheKeys.AdminTaxes];
}

public record CreateAdminTaxResult(Guid Id);

public class CreateAdminTaxCommandValidator : AbstractValidator<CreateAdminTaxCommand>
{
  public CreateAdminTaxCommandValidator()
  {
    RuleFor(x => x.Tax)
        .NotNull()
        .WithMessage("Tax data is required.");

    RuleFor(x => x.Tax.Rate)
        .InclusiveBetween(0, 100)
        .WithMessage("Tax rate must be between 0 and 100.");

    RuleFor(x => x.Tax.Translates)
        .NotEmpty()
        .WithMessage("At least one translation is required.");
  }
}

public class CreateAdminTaxHandler : ICommandHandler<CreateAdminTaxCommand, CreateAdminTaxResult>
{
  private readonly AccountingDbContext _context;

  public CreateAdminTaxHandler(AccountingDbContext context)
  {
    _context = context;
  }

  public async Task<CreateAdminTaxResult> Handle(CreateAdminTaxCommand request, CancellationToken cancellationToken)
  {
    if (request.Tax.Translates.Count == 0)
    {
      throw new TaxBadRequestException("At least one translation is required.");
    }

    // If the new Tax is being set as default, unset any existing default Tax
    if (request.Tax.IsDefault)
    {
      var existingDefault = await _context.Taxes
          .Where(t => t.IsDefault)
          .FirstOrDefaultAsync(cancellationToken);

      if (existingDefault != null)
      {
        existingDefault.UnsetAsDefault();
      }
    }

    var tax = Tax.Create(request.Tax.Rate, request.Tax.IsActive);

    // Set as default if requested
    if (request.Tax.IsDefault)
    {
      tax.SetAsDefault();
    }

    // Validate translations
    var validTranslations = request.Tax.Translates
        .Where(t => !string.IsNullOrWhiteSpace(t.Name) && t.LanguageId.HasValue)
        .ToList();

    // Check for existing Taxes with the same translation names
    var existingTaxNames = validTranslations.Select(t => t.Name).ToList();

    var existingTaxes = await _context.Taxes
        .Where(t => t.TaxTranslates.Any(tt => existingTaxNames.Contains(tt.Name)))
        .AsNoTracking()
        .ToListAsync(cancellationToken);

    if (existingTaxes.Any())
    {
      throw new TaxBadRequestException("A Tax with one of the provided names already exists.");
    }

    await _context.Taxes.AddAsync(tax, cancellationToken);

    // Add translations
    foreach (var transDto in validTranslations)
    {
      var newTranslate = TaxTranslate.Create(transDto.Name, transDto.Description, transDto.LanguageId, tax.Id);
      _context.TaxTranslates.Add(newTranslate);
    }

    await _context.SaveChangesAsync(cancellationToken);

    return new CreateAdminTaxResult(tax.Id);
  }
}