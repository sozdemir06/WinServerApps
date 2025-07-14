using Catalog.AppUnits.Models;


namespace Catalog.AppUnits.DomainEvents;

public record AppUnitCreatedEvent(AppUnit AppUnit) : IDomainEvent;