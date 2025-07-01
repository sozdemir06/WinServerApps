using Users.RoleGroups.DomainEvents;

namespace Users.RoleGroups.DomainEventHandlers;

public class RoleGroupActivatedEventHandler(ILogger<RoleGroupActivatedEventHandler> logger) : INotificationHandler<RoleGroupActivatedEvent>
{
  public async Task Handle(RoleGroupActivatedEvent notification, CancellationToken cancellationToken)
  {
    logger.LogInformation(
        "RoleGroupActivatedEvent handled => RoleGroupId: {RoleGroupId}",
        notification.RoleGroup.Id
    );

    // TODO: Implement any additional business logic for role group activation
    // For example:
    // - Send notifications
    // - Update related aggregates
    // - Trigger integration events
    // - Update cache
    // - etc.

    await Task.CompletedTask;
  }
}