using Microsoft.Extensions.Logging;
using Users.Managers.DomainEvents;

namespace Users.Managers.DomainEventHandlers;

public class ManagerDeactivatedEventHandler(ILogger<ManagerDeactivatedEventHandler> logger) : INotificationHandler<ManagerDeactivatedEvent>
{
  public Task Handle(ManagerDeactivatedEvent notification, CancellationToken cancellationToken)
  {
    logger.LogInformation(
        "[ManagerDeactivatedEventHandler] Manager deactivated: {ManagerId}, UserName: {UserName}",
        notification.Manager.Id,
        notification.Manager.UserName);

    return Task.CompletedTask;
  }
}