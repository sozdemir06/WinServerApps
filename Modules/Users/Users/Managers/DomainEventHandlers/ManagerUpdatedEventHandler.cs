using Microsoft.Extensions.Logging;
using Users.Managers.DomainEvents;

namespace Users.Managers.DomainEventHandlers;

public class ManagerUpdatedEventHandler(ILogger<ManagerUpdatedEventHandler> logger) : INotificationHandler<ManagerUpdatedEvent>
{
  public Task Handle(ManagerUpdatedEvent notification, CancellationToken cancellationToken)
  {
    logger.LogInformation(
        "[ManagerUpdatedEventHandler] Manager updated: {ManagerId}, UserName: {UserName}, Email: {Email}",
        notification.Manager.Id,
        notification.Manager.UserName,
        notification.Manager.Email);

    return Task.CompletedTask;
  }
}