using MassTransit;
using Microsoft.Extensions.Logging;
using Shared.Messages.Events.Users.Languages;
using Users.Languages.DomainEvents;

namespace Users.Languages.DomainEventHandlers;

public class LanguageCreatedEventHandler(ILogger<LanguageCreatedEventHandler> logger,IBus bus) : INotificationHandler<LanguageCreatedEvent>
{
  public async Task Handle(LanguageCreatedEvent notification, CancellationToken cancellationToken)
  {
    logger.LogInformation(
        "LanguageCreatedEvent handled => LanguageId: {LanguageId}, Name: {LanguageName}, Code: {LanguageCode}",
        notification.Language.Id,
        notification.Language.Name,
        notification.Language.Code);

    var language = notification.Language.Adapt<LanguageCreatedIntegrationEvent>();
    if(language!=null){
      await bus.Publish(language, cancellationToken); 
    }
    await Task.CompletedTask;
  }
}