namespace Users.AppRoles.QueryParams;

public record AppRoleQueryParams:PaginationParams
{
  public string? SearchTerm { get; set; }
  public bool? IsActive { get; set; }
  public string? SortBy { get; set; }
  public bool SortDescending { get; set; }
}