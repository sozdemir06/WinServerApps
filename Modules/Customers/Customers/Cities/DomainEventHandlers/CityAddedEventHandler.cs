using Customers.Cities.DomainEvents;


namespace Customers.Cities.DomainEventHandlers;

public class CityAddedEventHandler : INotificationHandler<CityAddedEvent>
{
  private readonly ILogger<CityAddedEventHandler> _logger;

  public CityAddedEventHandler(ILogger<CityAddedEventHandler> logger)
  {
    _logger = logger;
  }

  public Task Handle(CityAddedEvent notification, CancellationToken cancellationToken)
  {
    _logger.LogInformation("City {CityName} ({CityId}) was added to country {CountryName} ({CountryId})",
        notification.City.Name,
        notification.City.Id,
        notification.Country.Name,
        notification.Country.Id);

    return Task.CompletedTask;
  }
}