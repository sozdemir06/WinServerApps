using Users.AppRoles.Models;

namespace Users.AppRoles.DomainEvents;

public record AppRoleUpdatedEvent(AppRole Role) : IDomainEvent;