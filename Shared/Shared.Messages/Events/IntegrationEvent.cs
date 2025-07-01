

namespace Shared.Messages.Events;

public abstract record IntegrationEvent
{
  public Guid EventId { get; init; } = Guid.CreateVersion7();
  public DateTime OccurredOn { get; init; } = DateTime.UtcNow;
  public string EventType => GetType().AssemblyQualifiedName!;
}