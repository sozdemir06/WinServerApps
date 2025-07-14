using Catalog.AppUnits.Models;

namespace Catalog.AppUnits.DomainEvents;

public record AppUnitActivatedEvent(AppUnit AppUnit) : IDomainEvent;