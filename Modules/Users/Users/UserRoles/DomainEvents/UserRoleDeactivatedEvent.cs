using Users.UserRoles.Models;

namespace Users.UserRoles.DomainEvents;

public record UserRoleDeactivatedEvent(UserRole UserRole) : IDomainEvent;