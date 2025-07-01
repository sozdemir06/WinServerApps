using Customers.Currencies.Models;
using Shared.DDD;

namespace Customers.Currencies.DomainEvents;

public record CurrencyCreatedEvent(Currency Currency) : IDomainEvent;