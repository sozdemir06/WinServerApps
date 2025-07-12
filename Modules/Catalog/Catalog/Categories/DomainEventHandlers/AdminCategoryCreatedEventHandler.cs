using WinApps.Modules.Catalog.Catalog.Categories.DomainEvents;

namespace WinApps.Modules.Catalog.Catalog.Categories.DomainEventHandlers;

public class AdminCategoryCreatedEventHandler(ILogger<AdminCategoryCreatedEventHandler> logger) : INotificationHandler<AdminCategoryCreatedEvent>
{
  public async Task Handle(AdminCategoryCreatedEvent notification, CancellationToken cancellationToken)
  {
    logger.LogInformation(
        "AdminCategoryCreatedEvent handled => CategoryId: {CategoryId}, IsActive: {IsActive}, ParentId: {ParentId}",
        notification.Category.Id,
        notification.Category.IsActive,
        notification.Category.ParentId
    );

    // TODO: Implement any additional business logic for category creation
    // For example:
    // - Send notifications
    // - Update related aggregates
    // - Trigger integration events
    // - Update cache
    // - etc.

    await Task.CompletedTask;
  }
}