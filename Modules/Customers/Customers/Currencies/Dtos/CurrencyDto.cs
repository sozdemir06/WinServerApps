using System.Text.Json.Serialization;

namespace Customers.Currencies.Dtos;

public class CurrencyDto
{
  public long Id { get; init; }
  public string CurrencyCode { get; init; } = string.Empty;
  public string CurrencyName { get; init; } = string.Empty;
  public decimal? ForexBuying { get; init; } = 0;
  public decimal? ForexSelling { get; init; } = 0;
  public decimal? BanknoteBuying { get; init; } = 0;
  public decimal? BanknoteSelling { get; init; } = 0;
  public DateTime? Date { get; init; }
}