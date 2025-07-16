using Accounting.TaxGroups.Models;


namespace Accounting.TaxGroups.DomainEvents;

public record TaxGroupCreatedEvent(TaxGroup TaxGroup) : IDomainEvent;