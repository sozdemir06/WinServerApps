using Customers.Currencies.DomainEvents;
using Shared.Messages.Events.Customers.Currencies;

namespace Customers.Currencies.DomainEventHandlers;

public class CurrencyCreatedEventHandler(ILogger<CurrencyCreatedEventHandler> logger,IBus bus) : INotificationHandler<CurrencyCreatedEvent>
{

  public async Task Handle(CurrencyCreatedEvent notification, CancellationToken cancellationToken)
  {
    try
    {
      
      logger.LogInformation("MESSAGE [CURRENCY_CREATED] Currency created: {CurrencyCode} - {CurrencyName}",
        notification.Currency.CurrencyCode, notification.Currency.CurrencyName);
      var message = notification.Currency.Adapt<CurrencyCreatedIntegrationEvent>();
      if (message != null)
      {
        await bus.Publish(message, cancellationToken);
      }
    }
    catch (Exception ex) 
    {
      logger.LogError(ex, "Error handling CurrencyCreatedEvent");
    }
    await Task.CompletedTask;
  }
}