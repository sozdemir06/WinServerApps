

namespace Accounting.TaxGroups.QueryParams;

public record TaxGroupParams : PaginationParams
{
  public string? SearchTerm { get; set; }
  public bool? IsActive { get; set; }
  public bool? IsDefault { get; set; }
  public Guid? LanguageId { get; set; }
}