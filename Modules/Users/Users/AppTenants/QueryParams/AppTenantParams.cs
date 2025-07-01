namespace Users.AppTenants.QueryParams;

public record AppTenantParams : PaginationParams
{
 public string? Search { get; set; }
}