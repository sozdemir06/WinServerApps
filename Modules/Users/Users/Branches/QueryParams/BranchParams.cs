namespace WinApps.Modules.Users.Users.Branches.QueryParams;

public record BranchParams : PaginationParams
{
  public string? Search { get; init; }
  public bool? IsActive { get; init; }
  public string? Name { get; init; }
  public string? Code { get; init; }
}