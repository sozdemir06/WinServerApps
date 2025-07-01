using Customers.Districts.DomainEvents;


namespace Customers.Districts.DomainEventHandlers;

public class DistrictCreatedEventHandler : INotificationHandler<DistrictCreatedEvent>
{
  private readonly ILogger<DistrictCreatedEventHandler> _logger;

  public DistrictCreatedEventHandler(ILogger<DistrictCreatedEventHandler> logger)
  {
    _logger = logger;
  }

  public Task Handle(DistrictCreatedEvent notification, CancellationToken cancellationToken)
  {
    _logger.LogInformation("District {DistrictName} ({DistrictId}) was added to city {CityName} ({CityId})",
        notification.District.Name,
        notification.District.Id,
        notification.City.Name,
        notification.City.Id);

    return Task.CompletedTask;
  }
}