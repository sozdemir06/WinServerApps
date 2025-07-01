using FastEndpoints;
using Shared.Dtos;

namespace Shared.Services.Http.TestEndpoints
{
  public class ExchangeRateTestEndpoint : EndpointWithoutRequest<ExchangeRateResponseDto>
  {
    private readonly IExchangeRateService _exchangeRateService;
    private readonly ILogger<ExchangeRateTestEndpoint> _logger;

    public ExchangeRateTestEndpoint(
        IExchangeRateService exchangeRateService,
        ILogger<ExchangeRateTestEndpoint> logger)
    {
      _exchangeRateService = exchangeRateService;
      _logger = logger;
    }

    public override void Configure()
    {
      Get("/test/exchange-rates");
      AllowAnonymous();
    }

    public override async Task HandleAsync(CancellationToken ct)
    {
      try
      {
        _logger.LogInformation("Fetching exchange rates from TCMB");
        var result = await _exchangeRateService.GetExchangeRatesAsync(ct);

        await SendAsync(result, cancellation: ct);
      }
      catch (Exception ex)
      {
        _logger.LogError(ex, "Error fetching exchange rates");
        await SendAsync(new ExchangeRateResponseDto
        {
          ExchangeRates = [],
          LastUpdateTime = DateTime.Now,
          Source = "Error"
        }, cancellation: ct);
      }
    }
  }

  public class CurrencyConversionTestEndpoint : Endpoint<CurrencyConversionRequest, decimal>
  {
    private readonly IExchangeRateService _exchangeRateService;
    private readonly ILogger<CurrencyConversionTestEndpoint> _logger;

    public CurrencyConversionTestEndpoint(
        IExchangeRateService exchangeRateService,
        ILogger<CurrencyConversionTestEndpoint> logger)
    {
      _exchangeRateService = exchangeRateService;
      _logger = logger;
    }

    public override void Configure()
    {
      Post("/test/currency-conversion");
      AllowAnonymous();
    }

    public override async Task HandleAsync(CurrencyConversionRequest req, CancellationToken ct)
    {
      try
      {
        _logger.LogInformation("Converting {Amount} from {FromCurrency} to {ToCurrency}",
            req.Amount, req.FromCurrency, req.ToCurrency);

        var result = await _exchangeRateService.ConvertCurrencyAsync(
            req.FromCurrency, req.ToCurrency, req.Amount, ct);

        await SendAsync(result, cancellation: ct);
      }
      catch (Exception ex)
      {
        _logger.LogError(ex, "Error converting currency");
        await SendAsync(0, cancellation: ct);
      }
    }
  }

  public class CurrencyConversionRequest
  {
    public string FromCurrency { get; set; } = string.Empty;
    public string ToCurrency { get; set; } = string.Empty;
    public decimal Amount { get; set; }
  }
}