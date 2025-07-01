using Microsoft.Extensions.Logging;
using Users.Languages.DomainEvents;

namespace Users.Languages.DomainEventHandlers;

public class LanguageUpdatedEventHandler(ILogger<LanguageUpdatedEventHandler> logger) : INotificationHandler<LanguageUpdatedEvent>
{
  public async Task Handle(LanguageUpdatedEvent notification, CancellationToken cancellationToken)
  {
    logger.LogInformation(
        "LanguageUpdatedEvent handled => LanguageId: {LanguageId}, Name: {LanguageName}, Code: {LanguageCode}",
        notification.Language.Id,
        notification.Language.Name,
        notification.Language.Code);

    // TODO: Implement any additional business logic for language update
    // For example:
    // - Send notifications
    // - Update related aggregates
    // - Trigger integration events
    // - Update cache
    // - etc.

    await Task.CompletedTask;
  }
}