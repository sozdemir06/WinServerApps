using Accounting.TaxGroups.Dtos;
using Accounting.TaxGroups.Exceptions;
using Accounting.TaxGroups.Models;

namespace Accounting.TaxGroups.Features.TenantTaxGroups.UpdateTenantTaxGroup;

public record UpdateTenantTaxGroupCommand(Guid Id, TenantTaxGroupDto TenantTaxGroup) : ICommand<UpdateTenantTaxGroupResult>, ICacheRemovingRequest
{
  public List<string> CacheKeysToRemove => [CacheKeys.TenantTaxGroups];
}

public record UpdateTenantTaxGroupResult(Guid Id);

public class UpdateTenantTaxGroupCommandValidator : AbstractValidator<UpdateTenantTaxGroupCommand>
{
  public UpdateTenantTaxGroupCommandValidator()
  {
    RuleFor(x => x.Id)
        .NotEmpty()
        .WithMessage("TenantTaxGroup ID is required.");

    RuleFor(x => x.TenantTaxGroup)
        .NotNull()
        .WithMessage("TenantTaxGroup data is required.");

    RuleFor(x => x.TenantTaxGroup.Translates)
        .NotEmpty()
        .WithMessage("At least one translation is required.");

    RuleFor(x => x.TenantTaxGroup.TenantId)
        .NotNull()
        .WithMessage("TenantId is required.");
  }
}

public class UpdateTenantTaxGroupHandler : ICommandHandler<UpdateTenantTaxGroupCommand, UpdateTenantTaxGroupResult>
{
  private readonly AccountingDbContext _context;

  public UpdateTenantTaxGroupHandler(AccountingDbContext context)
  {
    _context = context;
  }

  public async Task<UpdateTenantTaxGroupResult> Handle(UpdateTenantTaxGroupCommand request, CancellationToken cancellationToken)
  {
    var tenantTaxGroup = await _context.TenantTaxGroups
        .Include(ttg => ttg.TenantTaxGroupTranslates)
        .FirstOrDefaultAsync(ttg => ttg.Id == request.Id, cancellationToken);

    if (tenantTaxGroup == null)
    {
      throw new TaxGroupNotFoundException(request.Id);
    }

    if (request.TenantTaxGroup.Translates.Count == 0)
    {
      throw new TaxGroupBadRequestException("At least one translation is required.");
    }

    // If the TenantTaxGroup is being set as default, unset any existing default TenantTaxGroup for this tenant
    if (request.TenantTaxGroup.IsDefault && !tenantTaxGroup.IsDefault)
    {
      var existingDefault = await _context.TenantTaxGroups
          .Where(ttg => ttg.IsDefault)
          .FirstOrDefaultAsync(cancellationToken);

      if (existingDefault != null)
      {
        existingDefault.UnsetAsDefault();
      }
    }

    // Update TenantTaxGroup properties
    tenantTaxGroup.Update(request.TenantTaxGroup.IsActive);

    if (request.TenantTaxGroup.IsDefault && !tenantTaxGroup.IsDefault)
    {
      tenantTaxGroup.SetAsDefault();
    }
    else if (!request.TenantTaxGroup.IsDefault && tenantTaxGroup.IsDefault)
    {
      tenantTaxGroup.UnsetAsDefault();
    }

    // Validate translations
    var validTranslations = request.TenantTaxGroup.Translates
        .Where(t => !string.IsNullOrWhiteSpace(t.Name) && t.LanguageId.HasValue)
        .ToList();

    // Check for existing TenantTaxGroups with the same translation names for this tenant (excluding current)
    var existingTenantTaxGroupNames = validTranslations.Select(t => t.Name).ToList();

    var existingTenantTaxGroups = await _context.TenantTaxGroups
        .Where(ttg => ttg.Id != request.Id  &&
                     ttg.TenantTaxGroupTranslates.Any(t => existingTenantTaxGroupNames.Contains(t.Name)))
        .AsNoTracking()
        .ToListAsync(cancellationToken);

    if (existingTenantTaxGroups.Any())
    {
      throw new TaxGroupBadRequestException("A TenantTaxGroup with one of the provided names already exists for this tenant.");
    }

    // Clear existing translations
    _context.TenantTaxGroupTranslates.RemoveRange(tenantTaxGroup.TenantTaxGroupTranslates);

    // Add new translations
    foreach (var transDto in validTranslations)
    {
      var newTranslate = TenantTaxGroupTranslate.Create(transDto.Name, transDto.Description, transDto.LanguageId, tenantTaxGroup.Id);
      _context.TenantTaxGroupTranslates.Add(newTranslate);
    }

    await _context.SaveChangesAsync(cancellationToken);

    return new UpdateTenantTaxGroupResult(tenantTaxGroup.Id);
  }
}