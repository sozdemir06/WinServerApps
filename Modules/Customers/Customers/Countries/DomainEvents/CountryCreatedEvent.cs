using Shared.DDD;

namespace Customers.Countries.DomainEvents;

public record CountryCreatedEvent(Country Country) : IDomainEvent;