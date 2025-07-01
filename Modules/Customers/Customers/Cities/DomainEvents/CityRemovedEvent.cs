using Customers.Cities.Models;
using Shared.DDD;

namespace Customers.Cities.DomainEvents;

public record CityRemovedEvent(City City) : IDomainEvent;