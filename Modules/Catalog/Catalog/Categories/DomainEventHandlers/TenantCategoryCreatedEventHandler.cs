using WinApps.Modules.Catalog.Catalog.Categories.DomainEvents;

namespace WinApps.Modules.Catalog.Catalog.Categories.DomainEventHandlers;

public class TenantCategoryCreatedEventHandler(ILogger<TenantCategoryCreatedEventHandler> logger) : INotificationHandler<TenantCategoryCreatedEvent>
{
  public async Task Handle(TenantCategoryCreatedEvent notification, CancellationToken cancellationToken)
  {
    logger.LogInformation(
        "TenantCategoryCreatedEvent handled => CategoryId: {CategoryId}, IsActive: {IsActive}, TenantId: {TenantId}, ParentId: {ParentId}",
        notification.Category.Id,
        notification.Category.IsActive,
        notification.Category.TenantId,
        notification.Category.ParentId
    );

    // TODO: Implement any additional business logic for tenant category creation
    // For example:
    // - Send notifications
    // - Update related aggregates
    // - Trigger integration events
    // - Update cache
    // - etc.

    await Task.CompletedTask;
  }
}