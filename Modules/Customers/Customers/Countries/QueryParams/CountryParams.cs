using Shared.Pagination;

namespace Customers.Countries.QueryParams;

public record CountryParams : PaginationParams
{
  public string? Search { get; init; }
  public string? SortBy { get; init; }
  public bool SortDescending { get; init; }
}