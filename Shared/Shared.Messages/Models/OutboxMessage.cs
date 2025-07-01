namespace Shared.Messages.Models;

public class OutboxMessage
{
  public Guid Id { get; set; } = default!;
  public string Type { get; set; } = default!;
  public string Content { get; set; } = default!;
  public DateTime CreatedAt { get; set; } = default!;
  public DateTime? ProcessedAt { get; set; } = default!;
  public int RetryCount { get; set; } = default!;
  public string? Error { get; set; } = default!;
}
