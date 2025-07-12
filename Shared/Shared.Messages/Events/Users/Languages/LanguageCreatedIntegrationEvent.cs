namespace Shared.Messages.Events.Users.Languages;

public record LanguageCreatedIntegrationEvent : IntegrationEvent
{
  public Guid Id { get; set; }
  public string Name { get; set; } = default!;
  public string Code { get; set; } = default!;
  public string? Description { get; set; }
  public bool IsDefault { get; set; }
  public bool IsActive { get; set; }
  public string? CreatedBy { get; set; }
  public DateTime CreatedAt { get; set; } = default!;
}