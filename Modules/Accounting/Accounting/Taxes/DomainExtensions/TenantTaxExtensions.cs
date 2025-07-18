using Accounting.Taxes.Dtos;
using Accounting.Taxes.Models;
using Accounting.Taxes.QueryParams;

namespace Accounting.Taxes.DomainExtensions;

public static class TenantTaxExtensions
{
  public static IQueryable<TenantTax> ApplyTenantTaxFilters(
      this IQueryable<TenantTax> query,
      TenantTaxParams parameters)
  {
    if (!string.IsNullOrWhiteSpace(parameters.Search))
    {
      var searchTerm = parameters.Search.ToLower();
      query = query.Where(tt =>
          tt.TenantTaxTranslates.Any(ttt => ttt.Name.ToLower().Contains(searchTerm)) ||
          tt.TenantTaxTranslates.Any(ttt => ttt.Description != null && ttt.Description.ToLower().Contains(searchTerm)));
    }

    if (parameters.IsActive.HasValue)
    {
      query = query.Where(tt => tt.IsActive == parameters.IsActive.Value);
    }

    if (!string.IsNullOrWhiteSpace(parameters.Name))
    {
      query = query.Where(tt => tt.TenantTaxTranslates.Any(t => t.Name.ToLower().Contains(parameters.Name.ToLower())));
    }


    return query;
  }

  public static IQueryable<TenantTax> ApplyTenantTaxOrdering(
      this IQueryable<TenantTax> query)
  {
    return query.OrderByDescending(tt => tt.CreatedAt);
  }

  public static TenantTaxDto ProjectTenantTaxToDto(this TenantTax tenantTax) =>
      new(
          tenantTax.Id,
          tenantTax.Rate,
          tenantTax.IsActive,
          tenantTax.IsDefault,
          tenantTax.TenantTaxGroupId,
          tenantTax.TenantId,
          tenantTax.TenantTaxTranslates.Select(ttt => ttt.ProjectTenantTaxTranslateToDto()).ToList(),
          tenantTax.CreatedBy,
          tenantTax.ModifiedBy,
          tenantTax.CreatedAt,
          tenantTax.UpdatedAt);

  public static IQueryable<TenantTaxDto> ProjectTenantTaxListToDto(this IQueryable<TenantTax> tenantTaxes) =>
    tenantTaxes.Select(tt => new TenantTaxDto(
        tt.Id,
        tt.Rate,
        tt.IsActive,
        tt.IsDefault,
        tt.TenantTaxGroupId,
        tt.TenantId,
        tt.TenantTaxTranslates.Select(ttt => ttt.ProjectTenantTaxTranslateToDto()).ToList(),
        tt.CreatedBy,
        tt.ModifiedBy,
        tt.CreatedAt,
        tt.UpdatedAt));

  public static TenantTaxTranslateDto ProjectTenantTaxTranslateToDto(this TenantTaxTranslate translate) =>
      new(
          translate.Id,
          translate.Name,
          translate.Description,
          translate.Language!.Code,
          translate.LanguageId,
          translate.TenantTaxId);

  public static bool IsValidTranslations(this TenantTax tenantTax)
  {
    return tenantTax.TenantTaxTranslates.Any();
  }

  public static bool IsActiveAndValid(this TenantTax tenantTax)
  {
    return tenantTax.IsActive && !tenantTax.IsDeleted && tenantTax.IsValidTranslations();
  }

  public static string GetDisplayName(this TenantTax tenantTax, string languageCode = "en")
  {
    var translation = tenantTax.TenantTaxTranslates.FirstOrDefault(ttt => ttt.LanguageId.HasValue);
    if (translation != null)
    {
      return translation.Name;
    }
    return tenantTax.Id.ToString();
  }
}