using Customers.Cities.DomainEvents;


namespace Customers.Cities.DomainEventHandlers;

public class CityUpdatedEventHandler : INotificationHandler<CityUpdatedEvent>
{
  private readonly ILogger<CityUpdatedEventHandler> _logger;

  public CityUpdatedEventHandler(ILogger<CityUpdatedEventHandler> logger)
  {
    _logger = logger;
  }

  public Task Handle(CityUpdatedEvent notification, CancellationToken cancellationToken)
  {
    _logger.LogInformation("City {CityName} ({CityId}) was updated",
        notification.City.Name,
        notification.City.Id);

    return Task.CompletedTask;
  }
}