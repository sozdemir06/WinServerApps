using Microsoft.Extensions.Logging;
using Users.AppRoles.DomainEvents;

namespace Users.AppRoles.DomainEventHandlers;

public class AppRoleUpdatedEventHandler(ILogger<AppRoleUpdatedEventHandler> logger) : INotificationHandler<AppRoleUpdatedEvent>
{
  public async Task Handle(AppRoleUpdatedEvent notification, CancellationToken cancellationToken)
  {
    logger.LogInformation(
        "AppRoleUpdatedEvent handled => RoleId: {RoleId}, Name: {RoleName}, NormalizedName: {NormalizedName}",
        notification.Role.Id,
        notification.Role.Name,
        notification.Role.NormalizedName);

    // TODO: Implement any additional business logic for role update
    // For example:
    // - Send notifications
    // - Update related aggregates
    // - Trigger integration events
    // - Update cache
    // - etc.

    await Task.CompletedTask;
  }
}