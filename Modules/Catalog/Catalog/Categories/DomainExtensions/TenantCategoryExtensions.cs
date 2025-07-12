using Catalog.AppTenants.DomainExtensions;
using Catalog.Categories.Models;
using WinApps.Modules.Catalog.Catalog.Categories.Dtos;
using WinApps.Modules.Catalog.Catalog.Categories.QueryParams;

namespace WinApps.Modules.Catalog.Catalog.Categories.DomainExtensions;

public static class TenantCategoryExtensions
{
  public static IQueryable<TenantCategory> ApplyTenantCategoryFilters(
      this IQueryable<TenantCategory> query,
      TenantCategoryParams parameters)
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

    if (parameters.ParentId.HasValue)
    {
      query = query.Where(c => c.ParentId == parameters.ParentId.Value);
    }

    if (parameters.TenantId.HasValue)
    {
      query = query.Where(c => c.TenantId == parameters.TenantId.Value);
    }

    return query;
  }

  public static IQueryable<TenantCategory> ApplyTenantCategoryOrdering(
      this IQueryable<TenantCategory> query)
  {
    return query.OrderBy(c => c.Translates.FirstOrDefault()!.Name);
  }

  public static TenantCategoryDto ProjectTenantCategoryToDto(this TenantCategory category) =>
      new(
          category.Id,
          category.IsActive,
          category.TenantId,
          category.AppTenant?.ProjectAppTenantToDto(),
          category.ParentId,
          category.Parent?.ProjectTenantCategoryToDto(),
          category.Children.Select(c => c.ProjectTenantCategoryToDto()).ToList(),
          category.Translates.Select(t => new TenantCategoryTranslateDto(
                t.Id,
                t.Name,
                t.Description,
                t.LanguageId,
                t.CategoryId)).ToList(),
          category.CreatedBy,
          category.ModifiedBy,
          category.CreatedAt,
          category.UpdatedAt);

  public static IQueryable<TenantCategoryDto> ProjectTenantCategoryListToDto(this IQueryable<TenantCategory> categories) =>
      categories.Select(c => new TenantCategoryDto(
          c.Id,
          c.IsActive,
          c.TenantId,
          null,
          c.ParentId,
          c.Parent != null ? new TenantCategoryDto(
              c.Parent.Id,
              c.Parent.IsActive,
              c.Parent.TenantId,
              null,
              c.Parent.ParentId,
              null,
              new List<TenantCategoryDto>(),
              c.Parent.Translates.Select(t => new TenantCategoryTranslateDto(
                  t.Id,
                  t.Name,
                  t.Description,
                  t.LanguageId,
                  t.CategoryId)).ToList(),
              c.Parent.CreatedBy,
              c.Parent.ModifiedBy,
              c.Parent.CreatedAt,
              c.Parent.UpdatedAt) : null,
          c.Children.Select(ch => new TenantCategoryDto(
              ch.Id,
              ch.IsActive,
              ch.TenantId,
              null,
              ch.ParentId,
              null,
              new List<TenantCategoryDto>(),
              new List<TenantCategoryTranslateDto>(),
              ch.CreatedBy,
              ch.ModifiedBy,
              ch.CreatedAt,
              ch.UpdatedAt)).ToList(),
          c.Translates.Select(t => new TenantCategoryTranslateDto(
              t.Id,
              t.Name,
              t.Description,
              t.LanguageId,
              t.CategoryId)).ToList(),
          c.CreatedBy,
          c.ModifiedBy,
          c.CreatedAt,
          c.UpdatedAt));
}