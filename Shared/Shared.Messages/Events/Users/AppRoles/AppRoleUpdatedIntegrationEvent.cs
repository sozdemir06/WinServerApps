namespace Shared.Messages.Events.Users.AppRoles;

public record AppRoleUpdatedIntegrationEvent : IntegrationEvent
{
  public Guid Id { get; set; }
  public string Name { get; set; } = default!;
  public string NormalizedName { get; set; } = default!;
  public string? Description { get; set; }
  public bool IsActive { get; set; }
  public string? CreatedBy { get; set; }
  public DateTime CreatedAt { get; set; }
  public DateTime? UpdatedAt { get; set; }
}