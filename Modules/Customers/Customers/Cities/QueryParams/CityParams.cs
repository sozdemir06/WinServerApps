using Shared.Pagination;

namespace Customers.Cities.QueryParams;

public record CityParams : PaginationParams
{
  public string? Search { get; set; }
  public long? CountryId { get; set; }
}