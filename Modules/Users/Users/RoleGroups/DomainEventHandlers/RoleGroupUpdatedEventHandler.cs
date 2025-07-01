using Users.RoleGroups.DomainEvents;

namespace Users.RoleGroups.DomainEventHandlers;

public class RoleGroupUpdatedEventHandler(ILogger<RoleGroupUpdatedEventHandler> logger) : INotificationHandler<RoleGroupUpdatedEvent>
{
  public async Task Handle(RoleGroupUpdatedEvent notification, CancellationToken cancellationToken)
  {
    logger.LogInformation(
        "RoleGroupUpdatedEvent handled => RoleGroupId: {RoleGroupId}, IsActive: {IsActive}",
        notification.RoleGroup.Id,
        notification.RoleGroup.IsActive
    );

    // TODO: Implement any additional business logic for role group update
    // For example:
    // - Send notifications
    // - Update related aggregates
    // - Trigger integration events
    // - Update cache
    // - etc.

    await Task.CompletedTask;
  }
}