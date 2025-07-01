using Shared.DDD;
using Users.RoleGroups.models;

namespace Users.RoleGroups.DomainEvents;

public record RoleGroupTranslationRemovedEvent(RoleGroup RoleGroup, RoleGroupTranslatate Translation) : IDomainEvent;