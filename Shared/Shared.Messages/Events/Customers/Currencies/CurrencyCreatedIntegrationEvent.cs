namespace Shared.Messages.Events.Customers.Currencies;

public record CurrencyCreatedIntegrationEvent : IntegrationEvent
{
  public long Id { get; set; }
  public string? CurrencyCode { get; set; } = string.Empty;
  public string? CurrencyName { get; set; } = string.Empty;
  public decimal? ForexBuying { get; set; } = 0;
  public decimal? ForexSelling { get; set; } = 0;
  public decimal? BanknoteBuying { get; set; } = 0;
  public decimal? BanknoteSelling { get; set; } = 0;
  public DateTime? Date { get; set; }
  public string? CreatedBy { get; set; }
  public DateTime CreatedAt { get; set; } = default!;
}