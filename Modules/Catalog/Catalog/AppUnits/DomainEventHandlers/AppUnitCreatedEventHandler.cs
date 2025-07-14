using Catalog.AppUnits.DomainEvents;

namespace Catalog.AppUnits.DomainEventHandlers;

public class AppUnitCreatedEventHandler(ILogger<AppUnitCreatedEventHandler> logger) : INotificationHandler<AppUnitCreatedEvent>
{
  public async Task Handle(AppUnitCreatedEvent notification, CancellationToken cancellationToken)
  {
    logger.LogInformation(
        "AppUnitCreatedEvent handled => AppUnitId: {AppUnitId}, MeasureUnitType: {MeasureUnitType}, IsActive: {IsActive}",
        notification.AppUnit.Id,
        notification.AppUnit.MeasureUnitType,
        notification.AppUnit.IsActive
    );

    // TODO: Implement any additional business logic for app unit creation
    // For example:
    // - Send notifications
    // - Update related aggregates
    // - Trigger integration events
    // - Update cache
    // - etc.

    await Task.CompletedTask;
  }
}