using Customers.Cities.Models;
using Shared.DDD;

namespace Customers.Cities.DomainEvents;

public record CityUpdatedEvent(City City) : IDomainEvent;