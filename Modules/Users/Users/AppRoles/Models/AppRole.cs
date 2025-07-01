using Users.AppRoles.DomainEvents;
using Users.Managers.Models;

namespace Users.AppRoles.Models;

public class AppRole : Aggregate<Guid>
{
  public string Name { get; private set; } = default!;
  public string NormalizedName { get; private set; } = default!;
  public string? Description { get; private set; }
  public RoleLanguageKey? RoleLanguageKey { get; private set; } = null;
  public bool IsActive { get; private set; } = default!;

  // Navigation Properties
  public ICollection<Manager> Managers { get; private set; } = default!;

  // Private constructor for EF Core
  private AppRole() { }

  public static AppRole Create(
      string name,
      string? description,
      RoleLanguageKey? roleLanguageKey)
  {
    ArgumentException.ThrowIfNullOrEmpty(name);
    var normalizedName = name.ToUpperInvariant();


    var role = new AppRole
    {
      Id = Guid.CreateVersion7(),
      Name = name,
      NormalizedName = normalizedName,
      Description = description,
      RoleLanguageKey = roleLanguageKey,
      IsActive = true,
      CreatedAt = DateTime.UtcNow
    };

    role.AddDomainEvent(new AppRoleCreatedEvent(role));

    return role;
  }

  public void Update(
      string name,
      string? description,
      RoleLanguageKey? roleLanguageKey)
  {
    ArgumentException.ThrowIfNullOrEmpty(name);
    var normalizedName = name.ToUpperInvariant();

    NormalizedName = normalizedName;
    Name = name;
    Description = description;
    RoleLanguageKey = roleLanguageKey;

    AddDomainEvent(new AppRoleUpdatedEvent(this));
  }

  public void Activate()
  {
    IsActive = true;
    AddDomainEvent(new AppRoleActivatedEvent(this));
  }

  public void Deactivate()
  {
    IsActive = false;
    AddDomainEvent(new AppRoleDeactivatedEvent(this));
  }
}