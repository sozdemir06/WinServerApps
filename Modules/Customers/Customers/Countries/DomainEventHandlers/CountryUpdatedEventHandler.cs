using Customers.Countries.DomainEvents;


namespace Customers.Countries.DomainEventHandlers;

public class CountryUpdatedEventHandler : INotificationHandler<CountryUpdatedEvent>
{
  private readonly ILogger<CountryUpdatedEventHandler> _logger;

  public CountryUpdatedEventHandler(ILogger<CountryUpdatedEventHandler> logger)
  {
    _logger = logger;
  }

  public Task Handle(CountryUpdatedEvent notification, CancellationToken cancellationToken)
  {
    _logger.LogInformation("Country {CountryName} ({CountryId}) was updated",
        notification.Country.Name,
        notification.Country.Id);

    return Task.CompletedTask;
  }
}