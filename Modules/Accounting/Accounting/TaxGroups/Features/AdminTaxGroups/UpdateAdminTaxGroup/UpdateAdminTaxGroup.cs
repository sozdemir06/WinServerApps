using Accounting.TaxGroups.Dtos;
using Accounting.TaxGroups.Exceptions;
using Accounting.TaxGroups.Models;

namespace Accounting.TaxGroups.Features.AdminTaxGroups.UpdateAdminTaxGroup;

public record UpdateAdminTaxGroupCommand(Guid Id, TaxGroupDto TaxGroup) : ICommand<UpdateAdminTaxGroupResult>, ICacheRemovingRequest
{
  public List<string> CacheKeysToRemove => [CacheKeys.AdminTaxGroups];
}

public record UpdateAdminTaxGroupResult(Guid Id);

public class UpdateAdminTaxGroupCommandValidator : AbstractValidator<UpdateAdminTaxGroupCommand>
{
  public UpdateAdminTaxGroupCommandValidator()
  {
    RuleFor(x => x.Id)
        .NotEmpty()
        .WithMessage("TaxGroup ID is required.");

    RuleFor(x => x.TaxGroup)
        .NotNull()
        .WithMessage("TaxGroup data is required.");

    RuleFor(x => x.TaxGroup.Translates)
        .NotEmpty()
        .WithMessage("At least one translation is required.");
  }
}

public class UpdateAdminTaxGroupHandler : ICommandHandler<UpdateAdminTaxGroupCommand, UpdateAdminTaxGroupResult>
{
  private readonly AccountingDbContext _context;

  public UpdateAdminTaxGroupHandler(AccountingDbContext context)
  {
    _context = context;
  }

  public async Task<UpdateAdminTaxGroupResult> Handle(UpdateAdminTaxGroupCommand request, CancellationToken cancellationToken)
  {
    var taxGroup = await _context.TaxGroups
        .Include(tg => tg.TaxGroupTranslates)
        .FirstOrDefaultAsync(tg => tg.Id == request.Id, cancellationToken);

    if (taxGroup == null)
    {
      throw new TaxGroupNotFoundException(request.Id);
    }

    if (request.TaxGroup.Translates.Count == 0)
    {
      throw new TaxGroupBadRequestException("At least one translation is required.");
    }

    // If the TaxGroup is being set as default, unset any existing default TaxGroup
    if (request.TaxGroup.IsDefault && !taxGroup.IsDefault)
    {
      var existingDefault = await _context.TaxGroups
          .Where(tg => tg.IsDefault && !tg.IsDeleted && tg.Id != request.Id)
          .FirstOrDefaultAsync(cancellationToken);

      if (existingDefault != null)
      {
        existingDefault.UnsetAsDefault();
      }
    }

    // Update TaxGroup properties
    taxGroup.Update(request.TaxGroup.IsActive);

    if (request.TaxGroup.IsDefault && !taxGroup.IsDefault)
    {
      taxGroup.SetAsDefault();
    }
    else if (!request.TaxGroup.IsDefault && taxGroup.IsDefault)
    {
      taxGroup.UnsetAsDefault();
    }

    // Validate translations
    var validTranslations = request.TaxGroup.Translates
        .Where(t => !string.IsNullOrWhiteSpace(t.Name) && t.LanguageId.HasValue)
        .ToList();

    // Check for existing TaxGroups with the same translation names (excluding current)
    var existingTaxGroupNames = validTranslations.Select(t => t.Name).ToList();

    var existingTaxGroups = await _context.TaxGroups
        .Where(tg => tg.Id != request.Id && tg.TaxGroupTranslates.Any(t => existingTaxGroupNames.Contains(t.Name)))
        .AsNoTracking()
        .ToListAsync(cancellationToken);

    if (existingTaxGroups.Any())
    {
      throw new TaxGroupBadRequestException("A TaxGroup with one of the provided names already exists.");
    }

    // Clear existing translations
    _context.TaxGroupTranslates.RemoveRange(taxGroup.TaxGroupTranslates);

    // Add new translations
    foreach (var transDto in validTranslations)
    {
      var newTranslate = TaxGroupTranslate.Create(transDto.Name, transDto.Description, transDto.LanguageId, taxGroup.Id);
      _context.TaxGroupTranslates.Add(newTranslate);
    }

    await _context.SaveChangesAsync(cancellationToken);

    return new UpdateAdminTaxGroupResult(taxGroup.Id);
  }
}