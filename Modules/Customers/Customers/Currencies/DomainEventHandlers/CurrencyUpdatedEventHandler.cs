using Customers.Currencies.DomainEvents;
using MediatR;

namespace Customers.Currencies.DomainEventHandlers;

public class CurrencyUpdatedEventHandler : INotificationHandler<CurrencyUpdatedEvent>
{
  private readonly ILogger<CurrencyUpdatedEventHandler> _logger;

  public CurrencyUpdatedEventHandler(ILogger<CurrencyUpdatedEventHandler> logger)
  {
    _logger = logger;
  }

  public Task Handle(CurrencyUpdatedEvent notification, CancellationToken cancellationToken)
  {
    _logger.LogInformation("Currency updated: {CurrencyCode} - {CurrencyName}",
        notification.Currency.CurrencyCode, notification.Currency.CurrencyName);

    return Task.CompletedTask;
  }
}