using Accounting.ExpensePens.Dtos;
using Accounting.ExpensePens.Models;
using Accounting.ExpensePens.QueryParams;

namespace Accounting.ExpensePens.DomainExtensions;

public static class TenantExpensePenExtensions
{
  public static IQueryable<TenantExpensePen> ApplyTenantExpensePenFilters(
      this IQueryable<TenantExpensePen> query,
      TenantExpensePenParams parameters)
  {

    if (!string.IsNullOrWhiteSpace(parameters.Search))
    {
      var searchTerm = parameters.Search.ToLower();
      query = query.Where(tep =>
          tep.TenantExpensePenTranslates.Any(t => t.Name.ToLower().Contains(searchTerm)) ||
          tep.TenantExpensePenTranslates.Any(t => t.Description != null && t.Description.ToLower().Contains(searchTerm)));
    }

    if (parameters.IsActive.HasValue)
    {
      query = query.Where(tep => tep.IsActive == parameters.IsActive.Value);
    }

    if (!string.IsNullOrWhiteSpace(parameters.Name))
    {
      query = query.Where(tep => tep.TenantExpensePenTranslates.Any(t => t.Name.ToLower().Contains(parameters.Name.ToLower())));
    }

    return query;
  }

  public static IQueryable<TenantExpensePen> ApplyTenantExpensePenOrdering(
      this IQueryable<TenantExpensePen> query)
  {
    return query.OrderByDescending(tep => tep.CreatedAt);
  }

  public static TenantExpensePenDto ProjectTenantExpensePenToDto(this TenantExpensePen tenantExpensePen) =>
      new(
          tenantExpensePen.Id,
          tenantExpensePen.IsActive,
          tenantExpensePen.IsDefault,
          tenantExpensePen.TenantExpensePenTranslates.Select(t => t.ProjectTenantExpensePenTranslateToDto()).ToList(),
          tenantExpensePen.CreatedBy,
          tenantExpensePen.ModifiedBy,
          tenantExpensePen.CreatedAt,
          tenantExpensePen.UpdatedAt);

  public static IQueryable<TenantExpensePenDto> ProjectTenantExpensePenListToDto(this IQueryable<TenantExpensePen> tenantExpensePens) =>
    tenantExpensePens.Select(tep => new TenantExpensePenDto(
        tep.Id,
        tep.IsActive,
        tep.IsDefault,
        tep.TenantExpensePenTranslates.Select(t => t.ProjectTenantExpensePenTranslateToDto()).ToList(),
        tep.CreatedBy,
        tep.ModifiedBy,
        tep.CreatedAt,
        tep.UpdatedAt));

  public static TenantExpensePenTranslateDto ProjectTenantExpensePenTranslateToDto(this TenantExpensePenTranslate translate) =>
      new(
          translate.Id,
          translate.Name,
          translate.Description,
          translate.Language!.Code,
          translate.LanguageId,
          translate.TenantExpensePenId);

  public static bool IsValidTranslations(this TenantExpensePen tenantExpensePen)
  {
    return tenantExpensePen.TenantExpensePenTranslates.Any();
  }

  public static bool IsActiveAndValid(this TenantExpensePen tenantExpensePen)
  {
    return tenantExpensePen.IsActive && !tenantExpensePen.IsDeleted && tenantExpensePen.IsValidTranslations();
  }

  public static string GetDisplayName(this TenantExpensePen tenantExpensePen, string languageCode = "en")
  {
    var translation = tenantExpensePen.TenantExpensePenTranslates.FirstOrDefault(t => t.LanguageId.HasValue);
    if (translation != null)
    {
      return translation.Name;
    }
    return tenantExpensePen.Id.ToString();
  }
}