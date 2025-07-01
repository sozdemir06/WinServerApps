using Shared.DDD;
using Users.RoleGroups.models;

namespace Users.RoleGroups.DomainEvents;

public record RoleGroupTranslationUpdatedEvent(RoleGroup RoleGroup, RoleGroupTranslatate Translation) : IDomainEvent;