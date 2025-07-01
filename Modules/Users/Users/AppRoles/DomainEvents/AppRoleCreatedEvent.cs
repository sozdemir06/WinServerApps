using Users.AppRoles.Models;

namespace Users.AppRoles.DomainEvents;

public record AppRoleCreatedEvent(AppRole Role) : IDomainEvent;