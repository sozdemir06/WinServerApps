using Accounting.TaxGroups.Dtos;
using Accounting.TaxGroups.Exceptions;
using Accounting.TaxGroups.Models;

namespace Accounting.TaxGroups.Features.AdminTaxGroups.CreateAdminTaxGroup;

public record CreateAdminTaxGroupCommand(TaxGroupDto TaxGroup) : ICommand<CreateAdminTaxGroupResult>, ICacheRemovingRequest
{
  public List<string> CacheKeysToRemove => [CacheKeys.AdminTaxGroups];
}

public record CreateAdminTaxGroupResult(Guid Id);

public class CreateAdminTaxGroupCommandValidator : AbstractValidator<CreateAdminTaxGroupCommand>
{
  public CreateAdminTaxGroupCommandValidator()
  {
    RuleFor(x => x.TaxGroup)
        .NotNull()
        .WithMessage("TaxGroup data is required.");

    RuleFor(x => x.TaxGroup.Translates)
        .NotEmpty()
        .WithMessage("At least one translation is required.");
  }
}

public class CreateAdminTaxGroupHandler : ICommandHandler<CreateAdminTaxGroupCommand, CreateAdminTaxGroupResult>
{
  private readonly AccountingDbContext _context;

  public CreateAdminTaxGroupHandler(AccountingDbContext context)
  {
    _context = context;
  }

  public async Task<CreateAdminTaxGroupResult> Handle(CreateAdminTaxGroupCommand request, CancellationToken cancellationToken)
  {
    if (request.TaxGroup.Translates.Count == 0)
    {
      throw new TaxGroupBadRequestException("At least one translation is required.");
    }

    // If the new TaxGroup is being set as default, unset any existing default TaxGroup
    if (request.TaxGroup.IsDefault)
    {
      var existingDefault = await _context.TaxGroups
          .Where(tg => tg.IsDefault)
          .FirstOrDefaultAsync(cancellationToken);

      if (existingDefault != null)
      {
        existingDefault.UnsetAsDefault();
      }
    }

    var taxGroup = TaxGroup.Create(request.TaxGroup.IsActive);

    // Set as default if requested
    if (request.TaxGroup.IsDefault)
    {
      taxGroup.SetAsDefault();
    }

    // Validate translations
    var validTranslations = request.TaxGroup.Translates
        .Where(t => !string.IsNullOrWhiteSpace(t.Name) && t.LanguageId.HasValue)
        .ToList();

    // Check for existing TaxGroups with the same translation names
    var existingTaxGroupNames = validTranslations.Select(t => t.Name).ToList();

    var existingTaxGroups = await _context.TaxGroups
        .Where(tg => tg.TaxGroupTranslates.Any(t => existingTaxGroupNames.Contains(t.Name)))
        .AsNoTracking()
        .ToListAsync(cancellationToken);

    if (existingTaxGroups.Any())
    {
      throw new TaxGroupBadRequestException("A TaxGroup with one of the provided names already exists.");
    }

    await _context.TaxGroups.AddAsync(taxGroup, cancellationToken);

    // Add translations
    foreach (var transDto in validTranslations)
    {
      var newTranslate = TaxGroupTranslate.Create(transDto.Name, transDto.Description, transDto.LanguageId, taxGroup.Id);
      _context.TaxGroupTranslates.Add(newTranslate);
    }

    await _context.SaveChangesAsync(cancellationToken);

    return new CreateAdminTaxGroupResult(taxGroup.Id);
  }
}