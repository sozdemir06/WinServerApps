using Customers.Districts.DomainEvents;
using Customers.Districts.Models;
using MediatR;
using Microsoft.Extensions.Logging;

namespace Customers.Districts.DomainEventHandlers;

public class DistrictUpdatedEventHandler : INotificationHandler<DistrictUpdatedEvent>
{
  private readonly ILogger<DistrictUpdatedEventHandler> _logger;

  public DistrictUpdatedEventHandler(ILogger<DistrictUpdatedEventHandler> logger)
  {
    _logger = logger;
  }

  public Task Handle(DistrictUpdatedEvent notification, CancellationToken cancellationToken)
  {
    _logger.LogInformation("District {DistrictName} ({DistrictId}) was updated",
        notification.District.Name,
        notification.District.Id);

    return Task.CompletedTask;
  }
}