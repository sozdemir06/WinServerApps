using Shared.Services.Http;

namespace Customers.Currencies.Features.UpdateExchangeRates;

public record UpdateExchangeRatesCommand : ICommand<UpdateExchangeRatesResult>, ICacheRemovingRequest
{
  public List<string> CacheKeysToRemove => [CacheKeys.Currencies];
}

public record UpdateExchangeRatesResult(int UpdatedCount, List<string> Errors);

public class UpdateExchangeRatesHandler : ICommandHandler<UpdateExchangeRatesCommand, UpdateExchangeRatesResult>
{
  private readonly CustomerDbContext _dbContext;
  private readonly IExchangeRateService _exchangeRateService;
  private readonly ILogger<UpdateExchangeRatesHandler> _logger;

  public UpdateExchangeRatesHandler(
      CustomerDbContext dbContext,
      IExchangeRateService exchangeRateService,
      ILogger<UpdateExchangeRatesHandler> logger)
  {
    _dbContext = dbContext;
    _exchangeRateService = exchangeRateService;
    _logger = logger;
  }

  public async Task<UpdateExchangeRatesResult> Handle(UpdateExchangeRatesCommand request, CancellationToken cancellationToken)
  {
    var errors = new List<string>();
    var updatedCount = 0;

    try
    {
      // Get current exchange rates from TCMB
      var exchangeRates = await _exchangeRateService.GetExchangeRatesAsync(cancellationToken);

      // Get all currencies from database (since we don't have IsActive property, we'll update all non-deleted currencies)
      var currencies = await _dbContext.Currencies
          .Where(c => !c.IsDeleted)
          .ToListAsync(cancellationToken);

      foreach (var currency in currencies)
      {
        try
        {
          // Find matching exchange rate from TCMB
          var exchangeRate = exchangeRates.ExchangeRates
              .FirstOrDefault(er => er.CurrencyCode.Equals(currency.CurrencyCode, StringComparison.OrdinalIgnoreCase));

          if (exchangeRate != null)
          {
            // Update currency with new exchange rates
            currency.UpdateExchangeRate(
                exchangeRate.ForexBuying,
                exchangeRate.ForexSelling,
                exchangeRate.BanknoteBuying,
                exchangeRate.BanknoteSelling
            );
            updatedCount++;

            _logger.LogInformation("Updated exchange rates for {CurrencyCode}: Buy={Buy}, Sell={Sell}",
                currency.CurrencyCode, exchangeRate.ForexBuying, exchangeRate.ForexSelling);
          }
          else
          {
            _logger.LogWarning("No exchange rate found for currency: {CurrencyCode}", currency.CurrencyCode);
          }
        }
        catch (Exception ex)
        {
          var error = $"Error updating {currency.CurrencyCode}: {ex.Message}";
          errors.Add(error);
          _logger.LogError(ex, "Error updating exchange rate for currency: {CurrencyCode}", currency.CurrencyCode);
        }
      }

      await _dbContext.SaveChangesAsync(cancellationToken);

      _logger.LogInformation("Exchange rates update completed. Updated: {UpdatedCount}, Errors: {ErrorCount}",
          updatedCount, errors.Count);
    }
    catch (Exception ex)
    {
      var error = $"Failed to fetch exchange rates: {ex.Message}";
      errors.Add(error);
      _logger.LogError(ex, "Failed to update exchange rates");
    }

    return new UpdateExchangeRatesResult(updatedCount, errors);
  }
}