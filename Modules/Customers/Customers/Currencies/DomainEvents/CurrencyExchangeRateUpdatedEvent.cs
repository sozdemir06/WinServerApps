using Customers.Currencies.Models;
using Shared.DDD;

namespace Customers.Currencies.DomainEvents;

public record CurrencyExchangeRateUpdatedEvent(Currency Currency) : IDomainEvent;