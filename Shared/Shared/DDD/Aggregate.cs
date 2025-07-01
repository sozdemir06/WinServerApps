namespace Shared.DDD;

using System.Collections.Generic;

public abstract class Aggregate<TKey> : Entity<TKey>, IAggregate<TKey>
{
  private readonly List<IDomainEvent> _domainEvents = [];
  public IReadOnlyList<IDomainEvent> DomainEvents => _domainEvents.AsReadOnly();

  public void AddDomainEvent(IDomainEvent domainEvent)
  {
    _domainEvents.Add(domainEvent);
  }

 
  public IDomainEvent[] ClearDomainEvents()
  {
    var events = _domainEvents.ToArray();
    _domainEvents.Clear();
    return events;
  }
}