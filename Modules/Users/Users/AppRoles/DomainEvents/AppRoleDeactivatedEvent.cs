using Users.AppRoles.Models;

namespace Users.AppRoles.DomainEvents;

public record AppRoleDeactivatedEvent(AppRole Role) : IDomainEvent;