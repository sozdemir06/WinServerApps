using Microsoft.Extensions.Logging;
using Users.AppRoles.DomainEvents;

namespace Users.AppRoles.DomainEventHandlers;

public class AppRoleActivatedEventHandler(ILogger<AppRoleActivatedEventHandler> logger) : INotificationHandler<AppRoleActivatedEvent>
{
  public async Task Handle(AppRoleActivatedEvent notification, CancellationToken cancellationToken)
  {
    logger.LogInformation(
        "AppRoleActivatedEvent handled => RoleId: {RoleId}, Name: {RoleName}, NormalizedName: {NormalizedName}",
        notification.Role.Id,
        notification.Role.Name,
        notification.Role.NormalizedName);

    // TODO: Implement any additional business logic for role activation
    // For example:
    // - Send notifications
    // - Update related aggregates
    // - Trigger integration events
    // - Update cache
    // - etc.

    await Task.CompletedTask;
  }
}