using Microsoft.Extensions.Logging;
using Users.AppRoles.DomainEvents;

namespace Users.AppRoles.DomainEventHandlers;

public class AppRoleDeactivatedEventHandler(ILogger<AppRoleDeactivatedEventHandler> logger) : INotificationHandler<AppRoleDeactivatedEvent>
{
  public async Task Handle(AppRoleDeactivatedEvent notification, CancellationToken cancellationToken)
  {
    logger.LogInformation(
        "AppRoleDeactivatedEvent handled => RoleId: {RoleId}, Name: {RoleName}, NormalizedName: {NormalizedName}",
        notification.Role.Id,
        notification.Role.Name,
        notification.Role.NormalizedName);

    // TODO: Implement any additional business logic for role deactivation
    // For example:
    // - Send notifications
    // - Update related aggregates
    // - Trigger integration events
    // - Update cache
    // - etc.

    await Task.CompletedTask;
  }
}