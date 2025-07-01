using Microsoft.Extensions.Logging;
using Users.Managers.DomainEvents;

namespace Users.Managers.DomainEventHandlers;

public class ManagerDeletedEventHandler(ILogger<ManagerDeletedEventHandler> logger) : INotificationHandler<ManagerDeletedEvent>
{
  public Task Handle(ManagerDeletedEvent notification, CancellationToken cancellationToken)
  {
    logger.LogInformation(
        "[ManagerDeletedEventHandler] Manager deleted: {ManagerId}, UserName: {UserName}",
        notification.Manager.Id,
        notification.Manager.UserName);

    return Task.CompletedTask;
  }
}