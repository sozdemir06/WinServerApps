using Shared.Pagination;

namespace Customers.Currencies.QueryParams;

public record CurrencyParams : PaginationParams
{
  public string? Search { get; init; }
  public string? SortBy { get; init; }
  public bool SortDescending { get; init; }
}