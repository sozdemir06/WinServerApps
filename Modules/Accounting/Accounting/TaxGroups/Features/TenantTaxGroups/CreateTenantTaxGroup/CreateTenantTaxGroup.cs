using Accounting.TaxGroups.Dtos;
using Accounting.TaxGroups.Exceptions;
using Accounting.TaxGroups.Models;

namespace Accounting.TaxGroups.Features.TenantTaxGroups.CreateTenantTaxGroup;

public record CreateTenantTaxGroupCommand(TenantTaxGroupDto TenantTaxGroup,Guid tenantId) : ICommand<CreateTenantTaxGroupResult>, ICacheRemovingRequest
{
  public List<string> CacheKeysToRemove => [CacheKeys.TenantTaxGroups];
}

public record CreateTenantTaxGroupResult(Guid Id);

public class CreateTenantTaxGroupCommandValidator : AbstractValidator<CreateTenantTaxGroupCommand>
{
  public CreateTenantTaxGroupCommandValidator()
  {
    RuleFor(x => x.TenantTaxGroup)
        .NotNull()
        .WithMessage("TenantTaxGroup data is required.");

    RuleFor(x => x.TenantTaxGroup.Translates)
        .NotEmpty()
        .WithMessage("At least one translation is required.");

    RuleFor(x => x.tenantId)
        .NotNull()
        .WithMessage("TenantId is required.");
  }
}

public class CreateTenantTaxGroupHandler : ICommandHandler<CreateTenantTaxGroupCommand, CreateTenantTaxGroupResult>
{
  private readonly AccountingDbContext _context;

  public CreateTenantTaxGroupHandler(AccountingDbContext context)
  {
    _context = context;
  }

  public async Task<CreateTenantTaxGroupResult> Handle(CreateTenantTaxGroupCommand request, CancellationToken cancellationToken)
  {
    if (request.TenantTaxGroup.Translates.Count == 0)
    {
      throw new TaxGroupBadRequestException("At least one translation is required.");
    }

    // If the new TenantTaxGroup is being set as default, unset any existing default TenantTaxGroup for this tenant
    if (request.TenantTaxGroup.IsDefault)
    {
      var existingDefault = await _context.TenantTaxGroups
          .Where(ttg => ttg.IsDefault)
          .FirstOrDefaultAsync(cancellationToken);

      if (existingDefault != null)
      {
        existingDefault.UnsetAsDefault();
      }
    }

    var tenantTaxGroup = TenantTaxGroup.Create(request.TenantTaxGroup.IsActive, request.tenantId);

    // Set as default if requested
    if (request.TenantTaxGroup.IsDefault)
    {
      tenantTaxGroup.SetAsDefault();
    }

    // Validate translations
    var validTranslations = request.TenantTaxGroup.Translates
        .Where(t => !string.IsNullOrWhiteSpace(t.Name) && t.LanguageId.HasValue)
        .ToList();

    // Check for existing TenantTaxGroups with the same translation names for this tenant
    var existingTenantTaxGroupNames = validTranslations.Select(t => t.Name).ToList();

    var existingTenantTaxGroups = await _context.TenantTaxGroups
        .Where(ttg => ttg.TenantId == request.tenantId &&
                     ttg.TenantTaxGroupTranslates.Any(t => existingTenantTaxGroupNames.Contains(t.Name)))
        .AsNoTracking()
        .ToListAsync(cancellationToken);

    if (existingTenantTaxGroups.Any())
    {
      throw new TaxGroupBadRequestException("A TenantTaxGroup with one of the provided names already exists for this tenant.");
    }

    await _context.TenantTaxGroups.AddAsync(tenantTaxGroup, cancellationToken);

    // Add translations
    foreach (var transDto in validTranslations)
    {
      var newTranslate = TenantTaxGroupTranslate.Create(transDto.Name, transDto.Description, transDto.LanguageId, tenantTaxGroup.Id);
      _context.TenantTaxGroupTranslates.Add(newTranslate);
    }

    await _context.SaveChangesAsync(cancellationToken);

    return new CreateTenantTaxGroupResult(tenantTaxGroup.Id);
  }
}