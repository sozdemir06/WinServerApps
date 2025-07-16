using Accounting.TaxGroups.Models;


namespace Accounting.TaxGroups.DomainEvents;

public record TaxGroupUpdatedEvent(TaxGroup TaxGroup) : IDomainEvent;