using Accounting.TaxGroups.Models;


namespace Accounting.TaxGroups.DomainEvents;

public record TaxGroupActivatedEvent(TaxGroup TaxGroup) : IDomainEvent;