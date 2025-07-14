using Catalog.AppUnits.Models;

namespace Catalog.AppUnits.DomainEvents;

public record AppUnitDeactivatedEvent(AppUnit AppUnit) : IDomainEvent;