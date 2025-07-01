using Customers.Districts.DomainEvents;


namespace Customers.Districts.DomainEventHandlers;

public class DistrictRemovedEventHandler : INotificationHandler<DistrictRemovedEvent>
{
  private readonly ILogger<DistrictRemovedEventHandler> _logger;

  public DistrictRemovedEventHandler(ILogger<DistrictRemovedEventHandler> logger)
  {
    _logger = logger;
  }

  public Task Handle(DistrictRemovedEvent notification, CancellationToken cancellationToken)
  {
    _logger.LogInformation("District {DistrictName} ({DistrictId}) was removed from city {CityName} ({CityId})",
        notification.District.Name,
        notification.District.Id,
        notification.City.Name,
        notification.City.Id);

    return Task.CompletedTask;
  }
}