using Accounting.Taxes.Dtos;
using Accounting.Taxes.Exceptions;
using Accounting.Taxes.Models;

namespace Accounting.Taxes.Features.AdminTaxes.UpdateAdminTax;

public record UpdateAdminTaxCommand(Guid Id, TaxDto Tax) : ICommand<UpdateAdminTaxResult>, ICacheRemovingRequest
{
  public List<string> CacheKeysToRemove => [CacheKeys.AdminTaxes];
}

public record UpdateAdminTaxResult(Guid Id);

public class UpdateAdminTaxCommandValidator : AbstractValidator<UpdateAdminTaxCommand>
{
  public UpdateAdminTaxCommandValidator()
  {
    RuleFor(x => x.Id)
        .NotEmpty()
        .WithMessage("Tax ID is required.");

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

public class UpdateAdminTaxHandler : ICommandHandler<UpdateAdminTaxCommand, UpdateAdminTaxResult>
{
  private readonly AccountingDbContext _context;

  public UpdateAdminTaxHandler(AccountingDbContext context)
  {
    _context = context;
  }

  public async Task<UpdateAdminTaxResult> Handle(UpdateAdminTaxCommand request, CancellationToken cancellationToken)
  {
    var tax = await _context.Taxes
        .Include(t => t.TaxTranslates)
        .FirstOrDefaultAsync(t => t.Id == request.Id, cancellationToken);

    if (tax == null)
    {
      throw new TaxNotFoundException(request.Id);
    }

    if (request.Tax.Translates.Count == 0)
    {
      throw new TaxBadRequestException("At least one translation is required.");
    }

    // If the Tax is being set as default, unset any existing default Tax
    if (request.Tax.IsDefault && !tax.IsDefault)
    {
      var existingDefault = await _context.Taxes
          .Where(t => t.IsDefault && !t.IsDeleted && t.Id != request.Id)
          .FirstOrDefaultAsync(cancellationToken);

      if (existingDefault != null)
      {
        existingDefault.UnsetAsDefault();
      }
    }

    // Update Tax properties
    tax.Update(request.Tax.Rate, request.Tax.IsActive);

    if (request.Tax.IsDefault && !tax.IsDefault)
    {
      tax.SetAsDefault();
    }
    else if (!request.Tax.IsDefault && tax.IsDefault)
    {
      tax.UnsetAsDefault();
    }

    // Validate translations
    var validTranslations = request.Tax.Translates
        .Where(t => !string.IsNullOrWhiteSpace(t.Name) && t.LanguageId.HasValue)
        .ToList();

    // Check for existing Taxes with the same translation names (excluding current)
    var existingTaxNames = validTranslations.Select(t => t.Name).ToList();

    var existingTaxes = await _context.Taxes
        .Where(t => t.Id != request.Id && t.TaxTranslates.Any(tt => existingTaxNames.Contains(tt.Name)))
        .AsNoTracking()
        .ToListAsync(cancellationToken);

    if (existingTaxes.Any())
    {
      throw new TaxBadRequestException("A Tax with one of the provided names already exists.");
    }

    // Clear existing translations
    _context.TaxTranslates.RemoveRange(tax.TaxTranslates);

    // Add new translations
    foreach (var transDto in validTranslations)
    {
      var newTranslate = TaxTranslate.Create(transDto.Name, transDto.Description, transDto.LanguageId, tax.Id);
      _context.TaxTranslates.Add(newTranslate);
    }

    await _context.SaveChangesAsync(cancellationToken);

    return new UpdateAdminTaxResult(tax.Id);
  }
}