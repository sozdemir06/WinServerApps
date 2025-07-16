namespace Accounting.Taxes.QueryParams;

public record TaxParams : PaginationParams
{
  public string? SearchTerm { get; set; }
  public bool? IsActive { get; set; }
  public bool? IsDefault { get; set; }
  public Guid? LanguageId { get; set; }
  public decimal? MinRate { get; set; }
  public decimal? MaxRate { get; set; }
}