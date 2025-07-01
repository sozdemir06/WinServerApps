using Shared.DDD;
using Users.Managers.Models;

namespace Users.Managers.DomainEvents;

public record ManagerDeletedEvent(Manager Manager) : IDomainEvent;