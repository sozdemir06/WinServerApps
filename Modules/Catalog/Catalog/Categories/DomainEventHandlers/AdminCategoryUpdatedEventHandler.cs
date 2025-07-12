using WinApps.Modules.Catalog.Catalog.Categories.DomainEvents;

namespace WinApps.Modules.Catalog.Catalog.Categories.DomainEventHandlers;

public class AdminCategoryUpdatedEventHandler(ILogger<AdminCategoryUpdatedEventHandler> logger) : INotificationHandler<AdminCategoryUpdatedEvent>
{
  public async Task Handle(AdminCategoryUpdatedEvent notification, CancellationToken cancellationToken)
  {
    logger.LogInformation(
        "AdminCategoryUpdatedEvent handled => CategoryId: {CategoryId}, IsActive: {IsActive}, ParentId: {ParentId}",
        notification.Category.Id,
        notification.Category.IsActive,
        notification.Category.ParentId
    );

    // TODO: Implement any additional business logic for category update
    // For example:
    // - Send notifications
    // - Update related aggregates
    // - Trigger integration events
    // - Update cache
    // - etc.

    await Task.CompletedTask;
  }
}