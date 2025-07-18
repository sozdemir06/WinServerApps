using Accounting.Taxes.DomainExtensions;
using Accounting.TaxGroups.Dtos;
using Accounting.TaxGroups.Models;
using Accounting.TaxGroups.QueryParams;

namespace Accounting.TaxGroups.DomainExtensions;

public static class TenantTaxGroupExtensions
{
  public static IQueryable<TenantTaxGroup> ApplyTenantTaxGroupFilters(
      this IQueryable<TenantTaxGroup> query,
      TenantTaxGroupParams parameters)
  {
    if (!string.IsNullOrWhiteSpace(parameters.Search))
    {
      var searchTerm = parameters.Search.ToLower();
      query = query.Where(ttg =>
          ttg.TenantTaxGroupTranslates.Any(t => t.Name.ToLower().Contains(searchTerm)) ||
          ttg.TenantTaxGroupTranslates.Any(t => t.Description != null && t.Description.ToLower().Contains(searchTerm)));
    }

    if (parameters.IsActive.HasValue)
    {
      query = query.Where(ttg => ttg.IsActive == parameters.IsActive.Value);
    }

    if (!string.IsNullOrWhiteSpace(parameters.Name))
    {
      query = query.Where(ttg => ttg.TenantTaxGroupTranslates.Any(t => t.Name.ToLower().Contains(parameters.Name.ToLower())));
    }


    return query;
  }

  public static IQueryable<TenantTaxGroup> ApplyTenantTaxGroupOrdering(
      this IQueryable<TenantTaxGroup> query)
  {
    return query.OrderByDescending(ttg => ttg.CreatedAt);
  }

  public static TenantTaxGroupDto ProjectTenantTaxGroupToDto(this TenantTaxGroup tenantTaxGroup) =>
      new(
          tenantTaxGroup.Id,
          tenantTaxGroup.IsActive,
          tenantTaxGroup.IsDefault,
          tenantTaxGroup.TenantId,
          tenantTaxGroup.TenantTaxGroupTranslates.Select(t => t.ProjectTenantTaxGroupTranslateToDto()).ToList(),
          tenantTaxGroup.TenantTaxes.Select(t => t.ProjectTenantTaxToDto()).ToList(),
          tenantTaxGroup.CreatedBy,
          tenantTaxGroup.ModifiedBy,
          tenantTaxGroup.CreatedAt,
          tenantTaxGroup.UpdatedAt);

  public static IQueryable<TenantTaxGroupDto> ProjectTenantTaxGroupListToDto(this IQueryable<TenantTaxGroup> tenantTaxGroups) =>
    tenantTaxGroups.Select(ttg => new TenantTaxGroupDto(
        ttg.Id,
        ttg.IsActive,
        ttg.IsDefault,
        ttg.TenantId,
        ttg.TenantTaxGroupTranslates.Select(t => t.ProjectTenantTaxGroupTranslateToDto()).ToList(),
        ttg.TenantTaxes.Select(t => t.ProjectTenantTaxToDto()).ToList(),
        ttg.CreatedBy,
        ttg.ModifiedBy,
        ttg.CreatedAt,
        ttg.UpdatedAt));

  public static TenantTaxGroupTranslateDto ProjectTenantTaxGroupTranslateToDto(this TenantTaxGroupTranslate translate) =>
      new(
          translate.Id,
          translate.Name,
          translate.Description,
          translate.Language!.Code,
          translate.LanguageId,
          translate.TenantTaxGroupId);

  public static bool IsValidTranslations(this TenantTaxGroup tenantTaxGroup)
  {
    return tenantTaxGroup.TenantTaxGroupTranslates.Any();
  }

  public static bool IsActiveAndValid(this TenantTaxGroup tenantTaxGroup)
  {
    return tenantTaxGroup.IsActive && !tenantTaxGroup.IsDeleted && tenantTaxGroup.IsValidTranslations();
  }

  public static string GetDisplayName(this TenantTaxGroup tenantTaxGroup, string languageCode = "en")
  {
    var translation = tenantTaxGroup.TenantTaxGroupTranslates.FirstOrDefault(t => t.LanguageId.HasValue);
    if (translation != null)
    {
      return translation.Name;
    }
    return tenantTaxGroup.Id.ToString();
  }
}