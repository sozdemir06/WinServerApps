
namespace Users.Languages.QueryParams;

public record GetLanguagesQueryParams : PaginationParams
{
  public string? Search { get; set; }
  public bool? IsActive { get; set; }
  public bool? IsDefault { get; set; }
}