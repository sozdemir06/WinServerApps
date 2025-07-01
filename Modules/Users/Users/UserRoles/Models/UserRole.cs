using Users.AppRoles.Models;
using Users.Managers.Models;
using Users.UserRoles.DomainEvents;

namespace Users.UserRoles.Models;

public class UserRole : Aggregate<Guid>
{
  public Guid ManagerId { get; private set; }
  public Guid RoleId { get; private set; }
  public bool IsActive { get; private set; }

  // Navigation Properties
  public Manager Manager { get; private set; } = default!;
  public AppRole AppRole { get; private set; } = default!;

  // Private constructor for EF Core
  private UserRole() { }

  public static UserRole Create(
      Guid managerId,
      Guid roleId)
  {
    ArgumentException.ThrowIfNullOrEmpty(managerId.ToString());
    ArgumentException.ThrowIfNullOrEmpty(roleId.ToString());

    var userRole = new UserRole
    {
      Id = Guid.CreateVersion7(),
      ManagerId = managerId,
      RoleId = roleId,
      IsActive = true,
      CreatedAt = DateTime.UtcNow
    };

    userRole.AddDomainEvent(new UserRoleCreatedEvent(userRole));

    return userRole;
  }

  public void Update(
      bool isActive)
  {
    IsActive = isActive;
    UpdatedAt = DateTime.UtcNow;

    AddDomainEvent(new UserRoleUpdatedEvent(this));
  }

  public void Activate()
  {
    if (IsActive)
    {
      return;
    }

    IsActive = true;
    UpdatedAt = DateTime.UtcNow;
    AddDomainEvent(new UserRoleActivatedEvent(this));
  }

  public void Deactivate()
  {
    if (!IsActive)
    {
      return;
    }

    IsActive = false;
    UpdatedAt = DateTime.UtcNow;
    AddDomainEvent(new UserRoleDeactivatedEvent(this));
  }
}