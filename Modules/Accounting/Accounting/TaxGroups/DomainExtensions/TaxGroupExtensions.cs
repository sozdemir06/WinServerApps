using Accounting.Taxes.DomainExtensions;
using Accounting.TaxGroups.Dtos;
using Accounting.TaxGroups.Models;
using Accounting.TaxGroups.QueryParams;

namespace Accounting.TaxGroups.DomainExtensions;

public static class TaxGroupExtensions
{
  public static IQueryable<TaxGroup> ApplyTaxGroupFilters(
      this IQueryable<TaxGroup> query,
      TaxGroupParams parameters)
  {
    if (!string.IsNullOrWhiteSpace(parameters.SearchTerm))
    {
      var searchTerm = parameters.SearchTerm.ToLower();
      query = query.Where(tg =>
          tg.TaxGroupTranslates.Any(t => t.Name.ToLower().Contains(searchTerm)) ||
          tg.TaxGroupTranslates.Any(t => t.Description != null && t.Description.ToLower().Contains(searchTerm)));
    }

    if (parameters.IsActive.HasValue)
    {
      query = query.Where(tg => tg.IsActive == parameters.IsActive.Value);
    }

    if (parameters.IsDefault.HasValue)
    {
      query = query.Where(tg => tg.IsDefault == parameters.IsDefault.Value);
    }

    if (parameters.LanguageId.HasValue)
    {
      query = query.Where(tg => tg.TaxGroupTranslates.Any(t => t.LanguageId == parameters.LanguageId.Value));
    }

    return query;
  }

  public static IQueryable<TaxGroup> ApplyTaxGroupOrdering(
      this IQueryable<TaxGroup> query)
  {
    return query.OrderByDescending(tg => tg.CreatedAt);
  }

  public static TaxGroupDto ProjectTaxGroupToDto(this TaxGroup taxGroup) =>
      new(
          taxGroup.Id,
          taxGroup.IsActive,
          taxGroup.IsDefault,
          taxGroup.TaxGroupTranslates.Select(t => t.ProjectTaxGroupTranslateToDto()).ToList(),
          taxGroup.CreatedBy,
          taxGroup.ModifiedBy,
          taxGroup.CreatedAt,
          taxGroup.UpdatedAt,
          taxGroup.Taxes.Select(t => t.ProjectTaxToDto()).ToList());

  public static IQueryable<TaxGroupDto> ProjectTaxGroupListToDto(this IQueryable<TaxGroup> taxGroups) =>
    taxGroups.Select(tg => new TaxGroupDto(
        tg.Id,
        tg.IsActive,
        tg.IsDefault,
        tg.TaxGroupTranslates.Select(t => t.ProjectTaxGroupTranslateToDto()).ToList(),
        tg.CreatedBy,
        tg.ModifiedBy,
        tg.CreatedAt,
        tg.UpdatedAt,
        tg.Taxes.Select(t => t.ProjectTaxToDto()).ToList()));

  public static TaxGroupTranslateDto ProjectTaxGroupTranslateToDto(this TaxGroupTranslate translate) =>
      new(
          translate.Id,
          translate.Name,
          translate.Description,
          translate.Language!.Code,
          translate.LanguageId,
          translate.TaxGroupId);

  public static bool IsValidTranslations(this TaxGroup taxGroup)
  {
    return taxGroup.TaxGroupTranslates.Any();
  }

  public static bool IsActiveAndValid(this TaxGroup taxGroup)
  {
    return taxGroup.IsActive && !taxGroup.IsDeleted && taxGroup.IsValidTranslations();
  }

  public static string GetDisplayName(this TaxGroup taxGroup, string languageCode = "en")
  {
    var translation = taxGroup.TaxGroupTranslates.FirstOrDefault(t => t.LanguageId.HasValue);
    if (translation != null)
    {
      return translation.Name;
    }
    return taxGroup.Id.ToString();
  }
}