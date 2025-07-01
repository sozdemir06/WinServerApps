using Users.Languages.Models;

namespace Users.Languages.DomainEvents;

public record LanguageCreatedEvent(Language Language) : IDomainEvent;