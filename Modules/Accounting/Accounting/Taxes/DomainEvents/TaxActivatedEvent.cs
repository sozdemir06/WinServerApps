using Accounting.Taxes.Models;

namespace Accounting.Taxes.DomainEvents;

public record TaxActivatedEvent(Tax Tax) : IDomainEvent;