using Microsoft.Extensions.Logging;
using Users.AppRoles.DomainEvents;

namespace Users.AppRoles.DomainEventHandlers;

public class AppRoleCreatedEventHandler(ILogger<AppRoleCreatedEventHandler> logger) : INotificationHandler<AppRoleCreatedEvent>
{
  public async Task Handle(AppRoleCreatedEvent notification, CancellationToken cancellationToken)
  {
    logger.LogInformation(
        "AppRoleCreatedEvent handled => RoleId: {RoleId}, Name: {RoleName}, NormalizedName: {NormalizedName}",
        notification.Role.Id,
        notification.Role.Name,
        notification.Role.NormalizedName);

    // TODO: Implement any additional business logic for role creation
    // For example:
    // - Send notifications
    // - Update related aggregates
    // - Trigger integration events
    // - Update cache
    // - etc.

    await Task.CompletedTask;
  }
}