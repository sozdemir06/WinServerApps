using Customers.Districts.Models;
using Shared.DDD;

namespace Customers.Districts.DomainEvents;

public record DistrictUpdatedEvent(District District) : IDomainEvent;