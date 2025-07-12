using Catalog.Categories.Models;
using WinApps.Modules.Catalog.Catalog.Categories.Dtos;
using WinApps.Modules.Catalog.Catalog.Categories.QueryParams;

namespace WinApps.Modules.Catalog.Catalog.Categories.DomainExtensions;

public static class AdminCategoryExtensions
{
  public static IQueryable<AdminCategory> ApplyAdminCategoryFilters(
    this IQueryable<AdminCategory> query,
    AdminCategoryParams parameters)
  {
    if (!string.IsNullOrWhiteSpace(parameters.Search))
    {
      var searchTerm = parameters.Search.ToLower();
      query = query.Where(c => c.Translates.Any(t =>
          t.Name.ToLower().Contains(searchTerm) ||
          t.Description != null && t.Description.ToLower().Contains(searchTerm)));
    }

    if (parameters.IsActive.HasValue)
    {
      query = query.Where(c => c.IsActive == parameters.IsActive.Value);
    }

    // Bu kısım sorununuzu çözmek için kritik
    if (parameters.ParentId.HasValue)
    {
      // Eğer belirli bir ebeveynin çocukları isteniyorsa, ona göre filtrele
      query = query.Where(c => c.ParentId == parameters.ParentId.Value);
    }
    else
    {
      // Eğer belirli bir ParentId verilmemişse, sadece en üst seviyedeki 
      // (ebeveyni olmayan) kategorileri getir.
      query = query.Where(c => c.ParentId == null);
    }

    return query;
  }

  public static IQueryable<AdminCategory> ApplyAdminCategoryOrdering(
      this IQueryable<AdminCategory> query)
  {
    return query.OrderBy(c => c.Translates.FirstOrDefault()!.Name);
  }

  public static AdminCategoryDto ProjectAdminCategoryToDto(this AdminCategory category) =>
    new(
        category.Id,
        category.IsActive,
        category.ParentId,
        category.Parent?.ProjectAdminCategoryToDto(),
        category.Translates.Select(t => new CategoryTranslateDto(
            t.LanguageId,
            t.Name,
            t.Description,
            t.Language!.Code)).ToList());

  public static IQueryable<AdminCategoryDto> ProjectAdminCategoryListToDto(this IQueryable<AdminCategory> categories) =>
      categories.Select(c => new AdminCategoryDto(      
          c.Id,
          c.IsActive,   
          c.ParentId,
          c.Parent != null ? new AdminCategoryDto(
              c.Parent.Id,
              c.Parent.IsActive,
              c.Parent.ParentId,
              null,
              c.Parent.Translates.Select(t => new CategoryTranslateDto( 
                  t.LanguageId,
                  t.Name,
                  t.Description,
                  t.Language!.Code)).ToList()) : null,
          c.Translates.Select(t => new CategoryTranslateDto(
              t.LanguageId,
              t.Name,
              t.Description, 
              t.Language!.Code)).ToList()));
}