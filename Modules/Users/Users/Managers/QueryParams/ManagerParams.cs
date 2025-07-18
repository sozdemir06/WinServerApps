
namespace Users.Managers.QueryParams;

public record ManagerParams : PaginationParams
{
  public string? Search { get; set; }
  public bool? IsActive { get; set; }
  public Guid? BranchId { get; set; }
  public string? Name { get; set; }
}