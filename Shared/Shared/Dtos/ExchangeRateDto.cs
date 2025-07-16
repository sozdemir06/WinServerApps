namespace Shared.Dtos
{
  public class ExchangeRateDto
  {
    public string CurrencyCode { get; set; } = string.Empty;
    public string CurrencyName { get; set; } = string.Empty;
    public decimal ForexBuying { get; set; }
    public decimal ForexSelling { get; set; }
    public decimal BanknoteBuying { get; set; }
    public decimal BanknoteSelling { get; set; }
    public string? CurrencySymbol { get; set; }
    public DateTime Date { get; set; }
  }

  public class ExchangeRateResponseDto
  {
    public List<ExchangeRateDto> ExchangeRates { get; set; } = [];
    public DateTime LastUpdateTime { get; set; }
    public string Source { get; set; } = "TCMB";
  }
}