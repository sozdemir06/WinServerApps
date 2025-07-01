using Users.UserRoles.Models;

namespace Users.UserRoles.DomainEvents;

public record UserRoleUpdatedEvent(UserRole UserRole) : IDomainEvent;