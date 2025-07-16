using Accounting.TaxGroups.Models;

namespace Accounting.TaxGroups.DomainEvents;

public record TaxGroupDeactivatedEvent(TaxGroup TaxGroup) : IDomainEvent;