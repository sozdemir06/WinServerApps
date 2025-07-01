using Users.RoleGroups.DomainEvents;

namespace Users.RoleGroups.DomainEventHandlers;

public class RoleGroupCreatedEventHandler(ILogger<RoleGroupCreatedEventHandler> logger) : INotificationHandler<RoleGroupCreatedEvent>
{
  public async Task Handle(RoleGroupCreatedEvent notification, CancellationToken cancellationToken)
  {
    logger.LogInformation(
        "RoleGroupCreatedEvent handled => RoleGroupId: {RoleGroupId}, IsActive: {IsActive}",
        notification.RoleGroup.Id,
        notification.RoleGroup.IsActive
    );

    // TODO: Implement any additional business logic for role group creation
    // For example:
    // - Send notifications
    // - Update related aggregates
    // - Trigger integration events
    // - Update cache
    // - etc.

    await Task.CompletedTask;
  }
}