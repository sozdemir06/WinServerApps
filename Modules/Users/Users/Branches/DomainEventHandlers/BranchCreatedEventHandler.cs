using WinApps.Modules.Users.Users.Branches.DomainEvents;

namespace WinApps.Modules.Users.Users.Branches.DomainEventHandlers;

public class BranchCreatedEventHandler(ILogger<BranchCreatedEventHandler> logger) : INotificationHandler<BranchCreatedEvent>
{
  public async Task Handle(BranchCreatedEvent notification, CancellationToken cancellationToken)
  {
    logger.LogInformation(
        "BranchCreatedEvent handled => BranchId: {BranchId}, Name: {BranchName}, Code: {BranchCode}, AppTenantId: {AppTenantId}",
        notification.Branch.Id,
        notification.Branch.Name,
        notification.Branch.Code,
        notification.Branch.TenantId
    );

    // TODO: Implement any additional business logic for branch creation
    // For example:
    // - Send notifications
    // - Update related aggregates
    // - Trigger integration events
    // - Update cache
    // - etc.

    await Task.CompletedTask;
  }
}