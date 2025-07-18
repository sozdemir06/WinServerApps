namespace Accounting.TaxGroups.QueryParams;

public record TenantTaxGroupParams : PaginationParams
{
  public string? Search { get; set; }
  public bool? IsActive { get; set; }
  public string? Name { get; set; }
}