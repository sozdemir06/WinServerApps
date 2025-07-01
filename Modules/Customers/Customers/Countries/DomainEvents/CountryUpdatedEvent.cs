using Customers.Countries.Models;
using Shared.DDD;

namespace Customers.Countries.DomainEvents;

public record CountryUpdatedEvent(Country Country) : IDomainEvent;