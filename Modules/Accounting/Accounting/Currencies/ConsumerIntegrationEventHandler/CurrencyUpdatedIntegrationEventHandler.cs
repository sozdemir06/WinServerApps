using Accounting.Currencies.Dtos;
using Accounting.Currencies.Features.UpdateCurrency;
using Shared.Messages.Events.Customers.Currencies;

namespace Accounting.Currencies.ConsumerIntegrationEventHandler;

public class CurrencyUpdatedIntegrationEventHandler(ISender sender, ILogger<CurrencyUpdatedIntegrationEventHandler> logger) : IConsumer<CurrencyUpdatedIntegrationEvent>
{
  public async Task Consume(ConsumeContext<CurrencyUpdatedIntegrationEvent> context)
  {
    try
    {
      // Create CurrencyDto manually to avoid mapping conflicts with base IntegrationEvent properties
      var currencyDto = new CurrencyDto
      {
        Id = context.Message.Id,
        CurrencyCode = context.Message.CurrencyCode,
        CurrencyName = context.Message.CurrencyName,
        ForexBuying = context.Message.ForexBuying,
        ForexSelling = context.Message.ForexSelling,
        BanknoteBuying = context.Message.BanknoteBuying,
        BanknoteSelling = context.Message.BanknoteSelling,
        Date = context.Message.Date,
        ModifiedBy = context.Message.ModifiedBy,
        CreatedBy = context.Message.ModifiedBy,
        CreatedAt = context.Message.ModifiedAt
      };

      var result = await sender.Send(new UpdateCurrencyCommand(currencyDto));
      if (result.Success)
      {
        logger.LogInformation("[CurrencyUpdatedIntegrationEvent] consumed: {CurrencyId}", context.Message.Id);
      }
    }
    catch (Exception ex)
    {
      logger.LogError(ex, "[CurrencyUpdatedIntegrationEvent] error: {CurrencyId}", context.Message.Id);
    }

    await Task.CompletedTask;
  }
}