using Catalog.AppUnits.Dtos;
using Catalog.AppUnits.Exceptions;
using Catalog.AppUnits.Models;
using FluentValidation;

namespace Catalog.AppUnits.Features.AdminAppUnits.UpdateAdminAppUnit;

public record UpdateAdminAppUnitCommand(Guid Id, AppUnitDto AppUnit) : ICommand<UpdateAdminAppUnitResult>, ICacheRemovingRequest
{
  public List<string> CacheKeysToRemove => [CacheKeys.AdminAppUnits];
}

public record UpdateAdminAppUnitResult(Guid Id);

public class UpdateAdminAppUnitCommandValidator : AbstractValidator<UpdateAdminAppUnitCommand>
{
  public UpdateAdminAppUnitCommandValidator()
  {
    RuleFor(x => x.AppUnit)
        .NotNull()
        .WithMessage("AppUnit data is required.");

    RuleFor(x => x.AppUnit.Translates)
        .NotEmpty()
        .WithMessage("At least one translation is required.");
  }
}

public class UpdateAdminAppUnitHandler : ICommandHandler<UpdateAdminAppUnitCommand, UpdateAdminAppUnitResult>
{
  private readonly CatalogDbContext _context;

  public UpdateAdminAppUnitHandler(CatalogDbContext context)
  {
    _context = context;
  }

  public async Task<UpdateAdminAppUnitResult> Handle(UpdateAdminAppUnitCommand request, CancellationToken cancellationToken)
  {
    if (request.AppUnit.Translates.Count == 0)
    {
      throw new AppUnitBadRequestException("At least one translation is required.");
    }

    var appUnit = await _context.AppUnits
        .Include(u => u.Translates)
        .FirstOrDefaultAsync(u => u.Id == request.Id && !u.IsDeleted, cancellationToken);

    if (appUnit == null)
    {
      throw new AppUnitNotFoundException($"AppUnit with ID '{request.Id}' not found.", request.Id);
    }

    // If the AppUnit is being set as default, unset any existing default AppUnit
    if (request.AppUnit.IsDefault && !appUnit.IsDefault)
    {
      var existingDefault = await _context.AppUnits
          .Where(u => u.IsDefault && !u.IsDeleted && u.Id != request.Id)
          .FirstOrDefaultAsync(cancellationToken);

      if (existingDefault != null)
      {
        existingDefault.UnsetAsDefault();
      }
    }

    appUnit.Update(request.AppUnit.MeasureUnitType, request.AppUnit.IsActive);

    // Set or unset as default
    if (request.AppUnit.IsDefault && !appUnit.IsDefault)
    {
      appUnit.SetAsDefault();
    }
    else if (!request.AppUnit.IsDefault && appUnit.IsDefault)
    {
      appUnit.UnsetAsDefault();
    }

    // Validate translations
    var validTranslations = request.AppUnit.Translates
        .Where(t => !string.IsNullOrWhiteSpace(t.Name) && t.LanguageId.HasValue)
        .ToList();

    // Check for existing AppUnits with the same translation names (excluding current AppUnit)
    var existingAppUnitNames = validTranslations.Select(t => t.Name).ToList();

    var existingAppUnits = await _context.AppUnits
        .Where(u => u.Id != request.Id && u.Translates.Any(t => existingAppUnitNames.Contains(t.Name)))
        .AsNoTracking()
        .ToListAsync(cancellationToken);

    if (existingAppUnits.Any())
    {
      throw new AppUnitBadRequestException("An AppUnit with one of the provided names already exists.");
    }

    // Remove existing translations from database
    _context.AppUnitTranslates.RemoveRange(appUnit.Translates);

    // Add new translations
    foreach (var transDto in validTranslations)
    {
      var newTranslate = AppUnitTranslate.Create(transDto.Name, transDto.Description, transDto.LanguageId, appUnit.Id);
      _context.AppUnitTranslates.Add(newTranslate);
    }

    await _context.SaveChangesAsync(cancellationToken);

    return new UpdateAdminAppUnitResult(appUnit.Id);
  }
}