using Customers.Currencies.DomainEvents;
using MediatR;

namespace Customers.Currencies.DomainEventHandlers;

public class CurrencyCreatedEventHandler : INotificationHandler<CurrencyCreatedEvent>
{
  private readonly ILogger<CurrencyCreatedEventHandler> _logger;

  public CurrencyCreatedEventHandler(ILogger<CurrencyCreatedEventHandler> logger)
  {
    _logger = logger;
  }

  public Task Handle(CurrencyCreatedEvent notification, CancellationToken cancellationToken)
  {
    _logger.LogInformation("Currency created: {CurrencyCode} - {CurrencyName}",
        notification.Currency.CurrencyCode, notification.Currency.CurrencyName);

    return Task.CompletedTask;
  }
}