using Catalog.AppUnits.DomainEvents;

namespace Catalog.AppUnits.DomainEventHandlers;

public class AppUnitActivatedEventHandler(ILogger<AppUnitActivatedEventHandler> logger) : INotificationHandler<AppUnitActivatedEvent>
{
  public async Task Handle(AppUnitActivatedEvent notification, CancellationToken cancellationToken)
  {
    logger.LogInformation(
        "AppUnitActivatedEvent handled => AppUnitId: {AppUnitId}, MeasureUnitType: {MeasureUnitType}",
        notification.AppUnit.Id,
        notification.AppUnit.MeasureUnitType
    );

    // TODO: Implement any additional business logic for app unit activation
    // For example:
    // - Send notifications
    // - Update related aggregates
    // - Trigger integration events
    // - Update cache
    // - etc.

    await Task.CompletedTask;
  }
}