using Microsoft.Extensions.Logging;
using Users.Managers.DomainEvents;

namespace Users.Managers.DomainEventHandlers;

public class ManagerCreatedEventHandler(ILogger<ManagerCreatedEventHandler> logger) : INotificationHandler<ManagerCreatedEvent>
{
  public Task Handle(ManagerCreatedEvent notification, CancellationToken cancellationToken)
  {
    logger.LogInformation(
        "[ManagerCreatedEventHandler] Manager created: {ManagerId}, UserName: {UserName}, Email: {Email}",
        notification.Manager.Id,
        notification.Manager.UserName,
        notification.Manager.Email);

    return Task.CompletedTask;
  }
}