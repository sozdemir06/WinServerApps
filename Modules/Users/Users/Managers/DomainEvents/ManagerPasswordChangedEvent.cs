using Shared.DDD;
using Users.Managers.Models;

namespace Users.Managers.DomainEvents;

public record ManagerPasswordChangedEvent(Manager Manager) : IDomainEvent;