using Accounting.Taxes.Dtos;
using Accounting.Taxes.Exceptions;
using Accounting.Taxes.Models;
using Accounting.TaxGroups.Exceptions;

namespace Accounting.Taxes.Features.AdminTaxes.AddTaxToTaxGroup;

public record AddTaxToTaxGroupCommand(Guid TaxGroupId, TaxDto Tax) : ICommand<AddTaxToTaxGroupResult>, ICacheRemovingRequest
{
  public List<string> CacheKeysToRemove => [CacheKeys.AdminTaxes, CacheKeys.AdminTaxGroups];
}

public record AddTaxToTaxGroupResult(Guid TaxId, Guid TaxGroupId);

public class AddTaxToTaxGroupCommandValidator : AbstractValidator<AddTaxToTaxGroupCommand>
{
  public AddTaxToTaxGroupCommandValidator()
  {
    RuleFor(x => x.TaxGroupId)
        .NotEmpty()
        .WithMessage("TaxGroup ID is required.");

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

public class AddTaxToTaxGroupHandler : ICommandHandler<AddTaxToTaxGroupCommand, AddTaxToTaxGroupResult>
{
  private readonly AccountingDbContext _context;

  public AddTaxToTaxGroupHandler(AccountingDbContext context)
  {
    _context = context;
  }

  public async Task<AddTaxToTaxGroupResult> Handle(AddTaxToTaxGroupCommand request, CancellationToken cancellationToken)
  {
    // Verify the tax group exists
    var taxGroup = await _context.TaxGroups
        .FirstOrDefaultAsync(tg => tg.Id == request.TaxGroupId, cancellationToken);

    if (taxGroup == null)
    {
      throw new TaxGroupNotFoundException(request.TaxGroupId);
    }

    if (request.Tax.Translates.Count == 0)
    {
      throw new TaxBadRequestException("At least one translation is required.");
    }

    // If the new Tax is being set as default, unset any existing default Tax in this tax group
    if (request.Tax.IsDefault)
    {
      var existingDefault = await _context.Taxes
          .Where(t => t.TaxGroupId == request.TaxGroupId && t.IsDefault)
          .FirstOrDefaultAsync(cancellationToken);

      if (existingDefault != null)
      {
        existingDefault.UnsetAsDefault();
      }
    }

    var tax = Tax.Create(request.Tax.Rate, request.Tax.IsActive);
    tax.UpdateTaxGroup(request.TaxGroupId);

    // Set as default if requested
    if (request.Tax.IsDefault)
    {
      tax.SetAsDefault();
    }

    // Validate translations
    var validTranslations = request.Tax.Translates
        .Where(t => !string.IsNullOrWhiteSpace(t.Name) && t.LanguageId.HasValue)
        .ToList();

    // Check for existing Taxes with the same translation names in this tax group
    var existingTaxNames = validTranslations.Select(t => t.Name).ToList();

    var existingTaxes = await _context.Taxes
        .Where(t => t.TaxGroupId == request.TaxGroupId && t.TaxTranslates.Any(tt => existingTaxNames.Contains(tt.Name)))
        .AsNoTracking()
        .ToListAsync(cancellationToken);

    if (existingTaxes.Any())
    {
      throw new TaxBadRequestException("A Tax with one of the provided names already exists in this tax group.");
    }

    await _context.Taxes.AddAsync(tax, cancellationToken);

    // Add translations
    foreach (var transDto in validTranslations)
    {
      var newTranslate = TaxTranslate.Create(transDto.Name, transDto.Description, transDto.LanguageId, tax.Id);
      _context.TaxTranslates.Add(newTranslate);
    }

    await _context.SaveChangesAsync(cancellationToken);

    return new AddTaxToTaxGroupResult(tax.Id, request.TaxGroupId);
  }
}