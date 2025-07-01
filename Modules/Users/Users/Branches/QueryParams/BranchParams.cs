namespace WinApps.Modules.Users.Users.Branches.QueryParams;

public record BranchParams : PaginationParams
{
  public string? Search { get; init; }
  public bool? IsActive { get; init; }
}