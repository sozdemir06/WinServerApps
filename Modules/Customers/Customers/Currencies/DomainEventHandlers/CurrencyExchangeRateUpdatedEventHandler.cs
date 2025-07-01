using Customers.Currencies.DomainEvents;
using MediatR;

namespace Customers.Currencies.DomainEventHandlers;

public class CurrencyExchangeRateUpdatedEventHandler : INotificationHandler<CurrencyExchangeRateUpdatedEvent>
{
  private readonly ILogger<CurrencyExchangeRateUpdatedEventHandler> _logger;

  public CurrencyExchangeRateUpdatedEventHandler(ILogger<CurrencyExchangeRateUpdatedEventHandler> logger)
  {
    _logger = logger;
  }

  public Task Handle(CurrencyExchangeRateUpdatedEvent notification, CancellationToken cancellationToken)
  {
    _logger.LogInformation("Currency exchange rate updated: {CurrencyCode} - {ExchangeRate}",
        notification.Currency.CurrencyCode, notification.Currency.ForexBuying);

    return Task.CompletedTask;
  }
}