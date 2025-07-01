using Users.Languages.Models;

namespace Users.Languages.DomainEvents;

public record LanguageUpdatedEvent(Language Language) : IDomainEvent;