using Catalog.AppUnits.Models;

namespace Catalog.AppUnits.DomainEvents;

public record AppUnitUpdatedEvent(AppUnit AppUnit) : IDomainEvent;