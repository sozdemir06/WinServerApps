using Users.UserRoles.Models;

namespace Users.UserRoles.DomainEvents;

public record UserRoleActivatedEvent(UserRole UserRole) : IDomainEvent;