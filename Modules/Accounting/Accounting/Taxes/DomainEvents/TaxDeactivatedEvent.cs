using Accounting.Taxes.Models;

namespace Accounting.Taxes.DomainEvents;

public record TaxDeactivatedEvent(Tax Tax) : IDomainEvent;