using Shared.DDD;
using Users.RoleGroups.models;

namespace Users.RoleGroups.DomainEvents;

public record RoleGroupUpdatedEvent(RoleGroup RoleGroup) : IDomainEvent;