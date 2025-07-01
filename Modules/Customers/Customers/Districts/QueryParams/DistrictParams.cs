using Shared.Pagination;

namespace Customers.Districts.QueryParams;

public record DistrictParams : PaginationParams
{
  public string? SearchTerm { get; set; }
  public long? CityId { get; set; }
  public long? CountryId { get; set; }
}