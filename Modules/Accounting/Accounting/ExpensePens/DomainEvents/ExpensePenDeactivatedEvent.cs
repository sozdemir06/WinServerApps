using Accounting.ExpensePens.Models;
using Shared.DDD;

namespace Accounting.ExpensePens.DomainEvents;

public record ExpensePenDeactivatedEvent(ExpensePen ExpensePen) : IDomainEvent;