using Users.RoleGroups.DomainEvents;

namespace Users.RoleGroups.DomainEventHandlers;

public class RoleGroupDeactivatedEventHandler(ILogger<RoleGroupDeactivatedEventHandler> logger) : INotificationHandler<RoleGroupDeactivatedEvent>
{
  public async Task Handle(RoleGroupDeactivatedEvent notification, CancellationToken cancellationToken)
  {
    logger.LogInformation(
        "RoleGroupDeactivatedEvent handled => RoleGroupId: {RoleGroupId}",
        notification.RoleGroup.Id
    );

    // TODO: Implement any additional business logic for role group deactivation
    // For example:
    // - Send notifications
    // - Update related aggregates
    // - Trigger integration events
    // - Update cache
    // - etc.

    await Task.CompletedTask;
  }
}