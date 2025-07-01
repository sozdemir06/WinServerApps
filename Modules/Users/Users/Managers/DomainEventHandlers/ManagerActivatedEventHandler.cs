using Microsoft.Extensions.Logging;
using Users.Managers.DomainEvents;

namespace Users.Managers.DomainEventHandlers;

public class ManagerActivatedEventHandler(ILogger<ManagerActivatedEventHandler> logger) : INotificationHandler<ManagerActivatedEvent>
{
  public Task Handle(ManagerActivatedEvent notification, CancellationToken cancellationToken)
  {
    logger.LogInformation(
        "[ManagerActivatedEventHandler] Manager activated: {ManagerId}, UserName: {UserName}",
        notification.Manager.Id,
        notification.Manager.UserName);

    return Task.CompletedTask;
  }
}