namespace Users.RoleGroups.QueryParams;

public record RoleGroupParams : PaginationParams
{
  public string? Search { get; init; }
  public bool? IsActive { get; init; }
  public Guid? LanguageId { get; init; }
}