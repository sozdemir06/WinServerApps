using MassTransit;
using Microsoft.Extensions.Logging;
using Shared.Messages.Events.Users.Languages;
using Users.Languages.DomainEvents;

namespace Users.Languages.DomainEventHandlers; 

public class LanguageUpdatedEventHandler(ILogger<LanguageUpdatedEventHandler> logger,IBus bus) : INotificationHandler<LanguageUpdatedEvent>
{
  public async Task Handle(LanguageUpdatedEvent notification, CancellationToken cancellationToken) 
  {
    logger.LogInformation(
        "LanguageUpdatedEvent handled => LanguageId: {LanguageId}, Name: {LanguageName}, Code: {LanguageCode}",
        notification.Language.Id,
        notification.Language.Name,
        notification.Language.Code);

    var language = notification.Language.Adapt<LanguageUpdatedIntegrationEvent>();
    if(language!=null){
      await bus.Publish(language, cancellationToken);
    }
    await Task.CompletedTask;
  }
}