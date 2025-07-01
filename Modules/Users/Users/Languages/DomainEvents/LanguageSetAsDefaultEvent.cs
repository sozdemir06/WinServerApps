using Users.Languages.Models;

namespace Users.Languages.DomainEvents;

public record LanguageSetAsDefaultEvent(Language Language) : IDomainEvent;