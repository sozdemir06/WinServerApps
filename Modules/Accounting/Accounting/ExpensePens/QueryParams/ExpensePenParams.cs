using Shared.Pagination;

namespace Accounting.ExpensePens.QueryParams;

public record ExpensePenParams : PaginationParams
{
  public string? SearchTerm { get; set; }
  public bool? IsActive { get; set; }
  public bool? IsDefault { get; set; }
  public Guid? LanguageId { get; set; }
}