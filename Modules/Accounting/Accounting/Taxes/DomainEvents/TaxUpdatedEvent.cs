using Accounting.Taxes.Models;

namespace Accounting.Taxes.DomainEvents;

public record TaxUpdatedEvent(Tax Tax) : IDomainEvent;