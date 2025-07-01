using WinApps.Modules.Users.Users.Branches.DomainEvents;

namespace WinApps.Modules.Users.Users.Branches.DomainEventHandlers;

public class BranchUpdatedEventHandler(ILogger<BranchUpdatedEventHandler> logger) : INotificationHandler<BranchUpdatedEvent>
{
  public async Task Handle(BranchUpdatedEvent notification, CancellationToken cancellationToken)
  {
    logger.LogInformation(
        "BranchUpdatedEvent handled => BranchId: {BranchId}, Name: {BranchName}, Code: {BranchCode}, AppTenantId: {AppTenantId}",
        notification.Branch.Id,
        notification.Branch.Name,
        notification.Branch.Code,
        notification.Branch.TenantId);

    // TODO: Implement any additional business logic for branch update
    // For example:
    // - Send notifications
    // - Update related aggregates
    // - Trigger integration events
    // - Update cache
    // - etc.

    await Task.CompletedTask;
  }
}