using Users.AppRoles.Models;

namespace Users.AppRoles.DomainEvents;

public record AppRoleActivatedEvent(AppRole Role) : IDomainEvent;