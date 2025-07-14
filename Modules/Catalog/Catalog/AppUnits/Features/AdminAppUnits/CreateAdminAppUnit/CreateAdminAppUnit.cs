using Catalog.AppUnits.Dtos;
using Catalog.AppUnits.Exceptions;
using Catalog.AppUnits.Models;

namespace Catalog.AppUnits.Features.AdminAppUnits.CreateAdminAppUnit;

public record CreateAdminAppUnitCommand(AppUnitDto AppUnit) : ICommand<CreateAdminAppUnitResult>, ICacheRemovingRequest
{
  public List<string> CacheKeysToRemove => [CacheKeys.AdminAppUnits];
}

public record CreateAdminAppUnitResult(Guid Id);

public class CreateAdminAppUnitCommandValidator : AbstractValidator<CreateAdminAppUnitCommand>
{
  public CreateAdminAppUnitCommandValidator()
  {
    RuleFor(x => x.AppUnit) 
        .NotNull()
        .WithMessage("AppUnit data is required.");

    RuleFor(x => x.AppUnit.Translates)
        .NotEmpty()
        .WithMessage("At least one translation is required.");
  }
}

public class CreateAdminAppUnitHandler : ICommandHandler<CreateAdminAppUnitCommand, CreateAdminAppUnitResult>
{
  private readonly CatalogDbContext _context;

  public CreateAdminAppUnitHandler(CatalogDbContext context)
  {
    _context = context;
  }

  public async Task<CreateAdminAppUnitResult> Handle(CreateAdminAppUnitCommand request, CancellationToken cancellationToken)
  {
    if (request.AppUnit.Translates.Count == 0)
    {
      throw new AppUnitBadRequestException("At least one translation is required.");
    }

    // If the new AppUnit is being set as default, unset any existing default AppUnit
    if (request.AppUnit.IsDefault)
    {
      var existingDefault = await _context.AppUnits
          .Where(u => u.IsDefault && !u.IsDeleted)
          .FirstOrDefaultAsync(cancellationToken);

      if (existingDefault != null)
      {
        existingDefault.UnsetAsDefault();
      }
    }

    var appUnit = AppUnit.Create(request.AppUnit.MeasureUnitType, request.AppUnit.IsActive);

    // Set as default if requested
    if (request.AppUnit.IsDefault)
    {
      appUnit.SetAsDefault();
    }

    // Validate translations
    var validTranslations = request.AppUnit.Translates
        .Where(t => !string.IsNullOrWhiteSpace(t.Name) && t.LanguageId.HasValue)
        .ToList();

    // Check for existing AppUnits with the same translation names
    var existingAppUnitNames = validTranslations.Select(t => t.Name).ToList();

    var existingAppUnits = await _context.AppUnits
        .Where(u => u.Translates.Any(t => existingAppUnitNames.Contains(t.Name)))
        .AsNoTracking()
        .ToListAsync(cancellationToken);

    if (existingAppUnits.Any())
    {
      throw new AppUnitBadRequestException("An AppUnit with one of the provided names already exists.");
    }

    await _context.AppUnits.AddAsync(appUnit, cancellationToken);

    // Add translations
    foreach (var transDto in validTranslations)
    {
      var newTranslate = AppUnitTranslate.Create(transDto.Name, transDto.Description, transDto.LanguageId, appUnit.Id);
      _context.AppUnitTranslates.Add(newTranslate);
    }

    await _context.SaveChangesAsync(cancellationToken);

    return new CreateAdminAppUnitResult(appUnit.Id);
  }
}