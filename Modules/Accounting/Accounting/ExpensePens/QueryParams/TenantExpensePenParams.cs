using Shared.Pagination;

namespace Accounting.ExpensePens.QueryParams;

public record TenantExpensePenParams : PaginationParams
{

  public string? Search { get; set; }
  public string? Name { get; set; }
  public bool? IsActive { get; set; }
}