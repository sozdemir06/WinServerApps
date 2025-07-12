using Customers.Currencies.DomainEvents;
using Shared.Messages.Events.Customers.Currencies;

namespace Customers.Currencies.DomainEventHandlers;

public class CurrencyUpdatedEventHandler(ILogger<CurrencyUpdatedEventHandler> logger,IBus bus) : INotificationHandler<CurrencyUpdatedEvent>
{

  public async Task Handle(CurrencyUpdatedEvent notification, CancellationToken cancellationToken)
  {
    try
    {
      logger.LogInformation("Currency updated: {CurrencyCode} - {CurrencyName}",
        notification.Currency.CurrencyCode, notification.Currency.CurrencyName);

      var message = notification.Currency.Adapt<CurrencyUpdatedIntegrationEvent>();
      if (message != null)
      {
        await bus.Publish(message, cancellationToken);
      }
    }
    catch (Exception ex)
    {
      logger.LogError(ex, "Error handling CurrencyUpdatedEvent");
    }
    await Task.CompletedTask;
  }
}