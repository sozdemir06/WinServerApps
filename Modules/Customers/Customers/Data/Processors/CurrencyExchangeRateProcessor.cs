using Customers.Currencies.Dtos;
using Customers.Currencies.Features.CreateCurrency;
using Customers.Currencies.Features.UpdateCurrency;
using Microsoft.Extensions.Hosting;
using Shared.Services.Http;

namespace Customers.Data.Processors;

public class CurrencyExchangeRateProcessor(
    ILogger<CurrencyExchangeRateProcessor> logger,
    IServiceProvider serviceProvider) : BackgroundService
{
  private readonly TimeSpan _interval = TimeSpan.FromMinutes(65);

  protected override async Task ExecuteAsync(CancellationToken stoppingToken)
  {
    while (!stoppingToken.IsCancellationRequested)
    {
      try
      {
        await ProcessExchangeRatesAsync(stoppingToken);
      }
      catch (Exception ex)
      {
        logger.LogError(ex, "Error occurred while processing exchange rates");
      }

      await Task.Delay(_interval, stoppingToken);
    }
  }

  private async Task ProcessExchangeRatesAsync(CancellationToken stoppingToken)
  {
    using var scope = serviceProvider.CreateScope();
    var dbContext = scope.ServiceProvider.GetRequiredService<CustomerDbContext>();
    var sender = scope.ServiceProvider.GetRequiredService<ISender>();
    var exchangeRateService = scope.ServiceProvider.GetRequiredService<IExchangeRateService>();

    try
    {
      logger.LogInformation("Starting exchange rate processing at {Time}", DateTime.UtcNow);

      // TCMB'den döviz kurlarını al
      var exchangeRatesResponse = await exchangeRateService.GetExchangeRatesAsync(stoppingToken);

      if (exchangeRatesResponse?.ExchangeRates == null || !exchangeRatesResponse.ExchangeRates.Any())
      {
        logger.LogWarning("No exchange rates received from TCMB");
        return;
      }

      // Mevcut para birimlerini veritabanından al
      var existingCurrencies = await dbContext.Currencies
          .Where(c => !c.IsDeleted)
          .ToListAsync(stoppingToken);

      var newCurrenciesCount = 0;
      var updatedCurrenciesCount = 0;

      foreach (var exchangeRate in exchangeRatesResponse.ExchangeRates)
      {
        try
        {
          // Mevcut para birimini bul
          var existingCurrency = existingCurrencies
              .FirstOrDefault(c => c.CurrencyCode!.Equals(exchangeRate.CurrencyCode, StringComparison.OrdinalIgnoreCase));

          // Yeni para birimi için DTO oluştur
          var currencyDto = new CurrencyDto
          {
            Id = existingCurrency?.Id ?? 0,
            CurrencyCode = exchangeRate.CurrencyCode,
            CurrencyName = exchangeRate.CurrencyName,
            ForexBuying = exchangeRate.ForexBuying,
            ForexSelling = exchangeRate.ForexSelling,
            BanknoteBuying = exchangeRate.BanknoteBuying,
            BanknoteSelling = exchangeRate.BanknoteSelling,
            Date = exchangeRate.Date
          };

          if (existingCurrency == null)
          {
            // Yeni para birimi - CreateCurrency command'ını gönder
            var createCommand = new CreateCurrencyCommand(currencyDto);
            var createResult = await sender.Send(createCommand, stoppingToken);

            newCurrenciesCount++;
            logger.LogInformation("Created new currency: {CurrencyCode} with ID: {Id}",
                exchangeRate.CurrencyCode, createResult.Id);
          }
          else
          {
            // Mevcut para birimi - UpdateCurrency command'ını gönder
            var updateCommand = new UpdateCurrencyCommand(existingCurrency.Id, currencyDto);
            var updateResult = await sender.Send(updateCommand, stoppingToken);

            updatedCurrenciesCount++;
            logger.LogInformation("Updated existing currency: {CurrencyCode} with ID: {Id}",
                exchangeRate.CurrencyCode, updateResult.Id);
          }
        }
        catch (Exception ex)
        {
          logger.LogError(ex, "Error processing currency {CurrencyCode}: {Message}",
              exchangeRate.CurrencyCode, ex.Message);
        }
      }

      logger.LogInformation("Exchange rate processing completed. New currencies: {NewCount}, Updated currencies: {UpdatedCount}",
          newCurrenciesCount, updatedCurrenciesCount);
    }
    catch (Exception ex)
    {
      logger.LogError(ex, "Error processing exchange rates");
    }
  }
}