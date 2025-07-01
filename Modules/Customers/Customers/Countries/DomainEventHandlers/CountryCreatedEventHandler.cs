using Customers.Countries.DomainEvents;


namespace Customers.Countries.DomainEventHandlers;

public class CountryCreatedEventHandler : INotificationHandler<CountryCreatedEvent>
{
  private readonly ILogger<CountryCreatedEventHandler> _logger;

  public CountryCreatedEventHandler(ILogger<CountryCreatedEventHandler> logger)
  {
    _logger = logger;
  }

  public Task Handle(CountryCreatedEvent notification, CancellationToken cancellationToken)
  {
    _logger.LogInformation("Country {CountryName} ({CountryId}) was created",
        notification.Country.Name,
        notification.Country.Id);

    return Task.CompletedTask;
  }
}