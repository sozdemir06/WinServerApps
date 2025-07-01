using Customers.Cities.DomainEvents;
using MediatR;

namespace Customers.Cities.DomainEventHandlers;

public class CityRemovedEventHandler : INotificationHandler<CityRemovedEvent>
{
  private readonly ILogger<CityRemovedEventHandler> _logger;

  public CityRemovedEventHandler(ILogger<CityRemovedEventHandler> logger)
  {
    _logger = logger;
  }

  public Task Handle(CityRemovedEvent notification, CancellationToken cancellationToken)
  {
    _logger.LogInformation("City {CityId} was removed", notification.City.Id);

    // Add any additional business logic here
    // For example: send notifications, update related data, etc.

    return Task.CompletedTask;
  }
}