using Microsoft.Extensions.Logging;
using WinApps.Modules.Users.Users.Branches.DomainEvents;

namespace WinApps.Modules.Users.Users.Branches.DomainEventHandlers;

public class BranchDeactivatedEventHandler(ILogger<BranchDeactivatedEventHandler> logger) : INotificationHandler<BranchDeactivatedEvent>
{
  public async Task Handle(BranchDeactivatedEvent notification, CancellationToken cancellationToken)
  {
    logger.LogInformation(
        "BranchDeactivatedEvent handled => BranchId: {BranchId}, Name: {BranchName}, Code: {BranchCode}, AppTenantId: {AppTenantId}",
        notification.Branch.Id,
        notification.Branch.Name,
        notification.Branch.Code,
        notification.Branch.TenantId);

    // TODO: Implement any additional business logic for branch deactivation
    // For example:
    // - Send notifications
    // - Update related aggregates
    // - Trigger integration events
    // - Update cache
    // - etc.

    await Task.CompletedTask;
  }
}