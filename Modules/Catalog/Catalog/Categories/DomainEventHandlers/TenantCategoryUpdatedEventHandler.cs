using WinApps.Modules.Catalog.Catalog.Categories.DomainEvents;

namespace WinApps.Modules.Catalog.Catalog.Categories.DomainEventHandlers;

public class TenantCategoryUpdatedEventHandler(ILogger<TenantCategoryUpdatedEventHandler> logger) : INotificationHandler<TenantCategoryUpdatedEvent>
{
  public async Task Handle(TenantCategoryUpdatedEvent notification, CancellationToken cancellationToken)
  {
    logger.LogInformation(
        "TenantCategoryUpdatedEvent handled => CategoryId: {CategoryId}, IsActive: {IsActive}, TenantId: {TenantId}, ParentId: {ParentId}",
        notification.Category.Id,
        notification.Category.IsActive,
        notification.Category.TenantId,
        notification.Category.ParentId
    );

    // TODO: Implement any additional business logic for tenant category update
    // For example:
    // - Send notifications
    // - Update related aggregates
    // - Trigger integration events
    // - Update cache
    // - etc.

    await Task.CompletedTask;
  }
}