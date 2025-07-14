using Accounting.ExpensePens.Dtos;
using Accounting.ExpensePens.Models;
using Accounting.ExpensePens.QueryParams;

namespace Accounting.ExpensePens.DomainExtensions;

public static class ExpensePenExtensions
{
  public static IQueryable<ExpensePen> ApplyExpensePenFilters(
      this IQueryable<ExpensePen> query,
      ExpensePenParams parameters)
  {
    if (!string.IsNullOrWhiteSpace(parameters.SearchTerm))
    {
      var searchTerm = parameters.SearchTerm.ToLower();
      query = query.Where(ep =>
          ep.ExpensePenTranslates.Any(t => t.Name.ToLower().Contains(searchTerm)) ||
          ep.ExpensePenTranslates.Any(t => t.Description != null && t.Description.ToLower().Contains(searchTerm)));
    }

    if (parameters.IsActive.HasValue)
    {
      query = query.Where(ep => ep.IsActive == parameters.IsActive.Value);
    }

    if (parameters.IsDefault.HasValue)
    {
      query = query.Where(ep => ep.IsDefault == parameters.IsDefault.Value);
    }

    if (parameters.LanguageId.HasValue)
    {
      query = query.Where(ep => ep.ExpensePenTranslates.Any(t => t.LanguageId == parameters.LanguageId.Value));
    }

    return query;
  }

  public static IQueryable<ExpensePen> ApplyExpensePenOrdering(
      this IQueryable<ExpensePen> query)
  {
    return query.OrderByDescending(ep => ep.CreatedAt);
  }

  public static ExpensePenDto ProjectExpensePenToDto(this ExpensePen expensePen) =>
      new(
          expensePen.Id,
          expensePen.IsActive,
          expensePen.IsDefault,
          expensePen.ExpensePenTranslates.Select(t => t.ProjectExpensePenTranslateToDto()).ToList(),
          expensePen.CreatedBy,
          expensePen.ModifiedBy,
          expensePen.CreatedAt,
          expensePen.UpdatedAt);

  public static IQueryable<ExpensePenDto> ProjectExpensePenListToDto(this IQueryable<ExpensePen> expensePens) =>
    expensePens.Select(ep => new ExpensePenDto(
        ep.Id,
        ep.IsActive,
        ep.IsDefault,
        ep.ExpensePenTranslates.Select(t => t.ProjectExpensePenTranslateToDto()).ToList(),
        ep.CreatedBy,
        ep.ModifiedBy,
        ep.CreatedAt,
        ep.UpdatedAt));

  public static ExpensePenTranslateDto ProjectExpensePenTranslateToDto(this ExpensePenTranslate translate) =>
      new(
          translate.Id,
          translate.Name,
          translate.Description,
          translate.Language!.Code,
          translate.LanguageId,
          translate.ExpensePenId);

  public static bool IsValidTranslations(this ExpensePen expensePen)
  {
    return expensePen.ExpensePenTranslates.Any();
  }

  public static bool IsActiveAndValid(this ExpensePen expensePen)
  {
    return expensePen.IsActive && !expensePen.IsDeleted && expensePen.IsValidTranslations();
  }

  public static string GetDisplayName(this ExpensePen expensePen, string languageCode = "en")
  {
    var translation = expensePen.ExpensePenTranslates.FirstOrDefault(t => t.LanguageId.HasValue);
    if (translation != null)
    {
      return translation.Name;
    }
    return expensePen.Id.ToString();
  }
}