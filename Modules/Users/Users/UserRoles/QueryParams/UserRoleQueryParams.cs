namespace Users.UserRoles.QueryParams;

public record UserRoleQueryParams : PaginationParams
{
  public Guid? ManagerId { get; set; }
  public Guid? RoleId { get; set; }
  public bool? IsActive { get; set; }
}