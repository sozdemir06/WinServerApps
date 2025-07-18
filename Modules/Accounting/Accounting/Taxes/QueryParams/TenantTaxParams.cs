namespace Accounting.Taxes.QueryParams;

public record TenantTaxParams : PaginationParams
{
  public string? Search { get; set; }
  public bool? IsActive { get; set; }
  public string? Name { get; set; }
}