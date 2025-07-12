using Catalog.Currencies.Dtos;
using Catalog.Currencies.Features.CreateCurrency;
using Shared.Messages.Events.Customers.Currencies;

namespace Catalog.Currencies.ConsumerIntegrationEventHandler;

public class CurrencyCreatedIntegrationEventHandler(ISender sender, ILogger<CurrencyCreatedIntegrationEventHandler> logger) : IConsumer<CurrencyCreatedIntegrationEvent>
{
  public async Task Consume(ConsumeContext<CurrencyCreatedIntegrationEvent> context)
  {
    try
    {
      var currencyDto = context.Message.Adapt<CurrencyDto>();
      var result = await sender.Send(new CreateCurrencyCommand(currencyDto));
      if (result.Success)
      {
        logger.LogInformation("[CURRENCY_CREATED] consumed: {CurrencyId}", context.Message.Id);
      }
    }
    catch (Exception ex)
    {
      logger.LogError(ex, "[CurrencyCreatedIntegrationEvent] error: {CurrencyId}", context.Message.Id);
    }

    await Task.CompletedTask;
  }
}