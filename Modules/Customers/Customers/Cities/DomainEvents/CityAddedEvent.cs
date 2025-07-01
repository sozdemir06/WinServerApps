using Customers.Cities.Models;
using Shared.DDD;

namespace Customers.Cities.DomainEvents;

public record CityAddedEvent(Country Country, City City) : IDomainEvent;