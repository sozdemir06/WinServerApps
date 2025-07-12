namespace Shared.Messages.Events.Users.Languages;

public record LanguageUpdatedIntegrationEvent : IntegrationEvent
{
  public Guid Id { get; set; }
  public string Name { get; set; } = default!;
  public string Code { get; set; } = default!;
  public string? Description { get; set; }
  public bool IsDefault { get; set; }
  public bool IsActive { get; set; }
  public DateTime? ModifiedAt { get; set; } = default!;
}