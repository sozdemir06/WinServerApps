using Shared.DDD;
using Users.RoleGroups.models;

namespace Users.RoleGroups.DomainEvents;

public record RoleGroupActivatedEvent(RoleGroup RoleGroup) : IDomainEvent;