using Users.UserRoles.Models;

namespace Users.UserRoles.DomainEvents;

public record UserRoleCreatedEvent(UserRole UserRole) : IDomainEvent;