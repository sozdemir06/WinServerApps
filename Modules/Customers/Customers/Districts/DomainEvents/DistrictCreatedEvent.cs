using Customers.Cities.Models;
using Customers.Districts.Models;
using Shared.DDD;

namespace Customers.Districts.DomainEvents;

public record DistrictCreatedEvent(City City, District District) : IDomainEvent;