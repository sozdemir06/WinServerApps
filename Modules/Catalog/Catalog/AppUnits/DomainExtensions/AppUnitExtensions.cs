using Catalog.AppUnits.Dtos;
using Catalog.AppUnits.Models;
using Catalog.AppUnits.QueryParams;

namespace Catalog.AppUnits.DomainExtensions;

public static class AppUnitExtensions
{
  public static IQueryable<AppUnit> ApplyAppUnitFilters(
      this IQueryable<AppUnit> query,
      AppUnitParams parameters)
  {
    if (!string.IsNullOrWhiteSpace(parameters.Search))
    {
      var searchTerm = parameters.Search.ToLower();
      query = query.Where(u =>
          u.Translates.Any(t => t.Name.ToLower().Contains(searchTerm)) ||
          u.Translates.Any(t => t.Description != null && t.Description.ToLower().Contains(searchTerm)));
    }

    if (parameters.IsActive.HasValue)
    {
      query = query.Where(u => u.IsActive == parameters.IsActive.Value);
    }

    if (parameters.MeasureUnitType.HasValue)
    {
      query = query.Where(u => u.MeasureUnitType == parameters.MeasureUnitType.Value);
    }

    return query;
  }

  public static IQueryable<AppUnit> ApplyAppUnitOrdering(
      this IQueryable<AppUnit> query)
  {
    return query.OrderByDescending(u => u.CreatedAt);
  }

  public static AppUnitDto ProjectAppUnitToDto(this AppUnit appUnit) =>
      new(
          appUnit.Id,
          appUnit.IsActive,
          appUnit.IsDefault,
          appUnit.MeasureUnitType,
          appUnit.Translates.Select(t => t.ProjectAppUnitTranslateToDto()).ToList(),
          appUnit.CreatedBy,
          appUnit.ModifiedBy,
          appUnit.CreatedAt,
          appUnit.UpdatedAt);

  public static IQueryable<AppUnitDto> ProjectAppUnitListToDto(this IQueryable<AppUnit> appUnits) =>
    appUnits.Select(u => new AppUnitDto(
        u.Id,
        u.IsActive,
        u.IsDefault,
        u.MeasureUnitType,
        u.Translates.Select(t => t.ProjectAppUnitTranslateToDto()).ToList(),
        u.CreatedBy,
        u.ModifiedBy,
        u.CreatedAt,
        u.UpdatedAt));

  public static AppUnitTranslateDto ProjectAppUnitTranslateToDto(this AppUnitTranslate translate) =>
      new(
          translate.Id,
          translate.Name,
          translate.Description,
          translate.Language?.Code,
          translate.LanguageId,
          translate.UnitId);
}