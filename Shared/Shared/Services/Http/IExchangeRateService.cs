using Shared.Dtos;

namespace Shared.Services.Http
{
  public interface IExchangeRateService
  {
    Task<ExchangeRateResponseDto> GetExchangeRatesAsync(CancellationToken cancellationToken = default);
    Task<ExchangeRateDto?> GetExchangeRateByCurrencyAsync(string currencyCode, CancellationToken cancellationToken = default);
    Task<decimal> ConvertCurrencyAsync(string fromCurrency, string toCurrency, decimal amount, CancellationToken cancellationToken = default);
  }
}