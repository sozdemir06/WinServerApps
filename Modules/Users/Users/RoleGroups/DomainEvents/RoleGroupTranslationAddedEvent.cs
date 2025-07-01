using Users.RoleGroups.models;

namespace Users.RoleGroups.DomainEvents;

public record RoleGroupTranslationAddedEvent(RoleGroup RoleGroup, RoleGroupTranslatate Translation) : IDomainEvent;