using Shared.Pagination;

namespace Users.Managers.QueryParams;

public record ManagerParams : PaginationParams
{
  public string? Search { get; set; }
  public bool? IsActive { get; set; }
  public bool? IsAdmin { get; set; }
  public bool? IsManager { get; set; }
  public Guid? TenantId { get; set; }
  public Guid? BranchId { get; set; }
  public string? SortBy { get; set; }
  public bool IsDescending { get; set; } = false;
}