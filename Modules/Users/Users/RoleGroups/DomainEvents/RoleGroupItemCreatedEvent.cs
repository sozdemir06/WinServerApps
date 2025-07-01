using Shared.DDD;
using Users.RoleGroups.models;

namespace Users.RoleGroups.DomainEvents;

public record RoleGroupItemCreatedEvent(RoleGroupItem RoleGroupItem) : IDomainEvent;