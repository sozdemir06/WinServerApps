using Accounting.Taxes.Models;

namespace Accounting.Taxes.DomainEvents;

public record TaxCreatedEvent(Tax Tax) : IDomainEvent;