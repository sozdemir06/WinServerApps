using Accounting.Taxes.Dtos;
using Accounting.Taxes.Models;
using Accounting.Taxes.QueryParams;

namespace Accounting.Taxes.DomainExtensions;

public static class TaxExtensions
{
  public static IQueryable<Tax> ApplyTaxFilters(
      this IQueryable<Tax> query,
      TaxParams parameters)
  {
    if (!string.IsNullOrWhiteSpace(parameters.SearchTerm))
    {
      var searchTerm = parameters.SearchTerm.ToLower();
      query = query.Where(t =>
          t.TaxTranslates.Any(tt => tt.Name.ToLower().Contains(searchTerm)) ||
          t.TaxTranslates.Any(tt => tt.Description != null && tt.Description.ToLower().Contains(searchTerm)));
    }

    if (parameters.IsActive.HasValue)
    {
      query = query.Where(t => t.IsActive == parameters.IsActive.Value);
    }

    if (parameters.IsDefault.HasValue)
    {
      query = query.Where(t => t.IsDefault == parameters.IsDefault.Value);
    }

    if (parameters.LanguageId.HasValue)
    {
      query = query.Where(t => t.TaxTranslates.Any(tt => tt.LanguageId == parameters.LanguageId.Value));
    }

    if (parameters.MinRate.HasValue)
    {
      query = query.Where(t => t.Rate >= parameters.MinRate.Value);
    }

    if (parameters.MaxRate.HasValue)
    {
      query = query.Where(t => t.Rate <= parameters.MaxRate.Value);
    }

    return query;
  }

  public static IQueryable<Tax> ApplyTaxOrdering(
      this IQueryable<Tax> query)
  {
    return query.OrderByDescending(t => t.CreatedAt);
  }

  public static TaxDto ProjectTaxToDto(this Tax tax) =>
      new(
          tax.Id,
          tax.Rate,
          tax.IsActive,
          tax.IsDefault,
          tax.TaxGroupId,
          tax.TaxTranslates.Select(tt => tt.ProjectTaxTranslateToDto()).ToList(),
          tax.CreatedBy,
          tax.ModifiedBy,
          tax.CreatedAt,
          tax.UpdatedAt);

  public static IQueryable<TaxDto> ProjectTaxListToDto(this IQueryable<Tax> taxes) =>
    taxes.Select(t => new TaxDto(
        t.Id,
        t.Rate,
        t.IsActive,
        t.IsDefault,
        t.TaxGroupId,
        t.TaxTranslates.Select(tt => tt.ProjectTaxTranslateToDto()).ToList(),
        t.CreatedBy,
        t.ModifiedBy,
        t.CreatedAt,
        t.UpdatedAt));

  public static TaxTranslateDto ProjectTaxTranslateToDto(this TaxTranslate translate) =>
      new(
          translate.Id,
          translate.Name,
          translate.Description,
          translate.Language!.Code,
          translate.LanguageId,
          translate.TaxId);

  public static bool IsValidTranslations(this Tax tax)
  {
    return tax.TaxTranslates.Any();
  }

  public static bool IsActiveAndValid(this Tax tax)
  {
    return tax.IsActive && !tax.IsDeleted && tax.IsValidTranslations();
  }

  public static string GetDisplayName(this Tax tax, string languageCode = "en")
  {
    var translation = tax.TaxTranslates.FirstOrDefault(tt => tt.LanguageId.HasValue);
    if (translation != null)
    {
      return translation.Name;
    }
    return tax.Id.ToString();
  }
}