using Shared.Pagination;


namespace WinApps.Modules.Catalog.Catalog.Categories.QueryParams;

public record AdminCategoryParams : PaginationParams
{
  public string? Search { get; init; }
  public bool? IsActive { get; init; }
  public Guid? ParentId { get; init; }
}