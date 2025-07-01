using Users.Managers.Models;
using WinApps.Modules.Users.Users.Branches.DomainEvents;

namespace WinApps.Modules.Users.Users.Branches.Models;

public class Branch : Aggregate<Guid>
{
  public string Name { get; private set; } = default!;
  public string Code { get; private set; } = default!;
  public string? Address { get; private set; }
  public string? Phone { get; private set; }
  public string? Email { get; private set; }
  public bool IsActive { get; private set; }
  public string? Description { get; private set; }
  public Guid TenantId { get; private set; }

  // Navigation Properties
  public AppTenant AppTenant { get; private set; } = default!;
  public ICollection<Manager> Managers { get; private set; } = [];

  // Private constructor for EF Core
  private Branch() { }

  public static Branch Create(
      string name,
      string code,
      string? address,
      string? phone,
      string? email,
      bool isActive,
      string? description,
      Guid appTenantId)
  {
    ArgumentException.ThrowIfNullOrEmpty(name);
    ArgumentException.ThrowIfNullOrEmpty(code);

    var branch = new Branch
    {
      Id = Guid.CreateVersion7(),
      Name = name,
      Code = code,
      Address = address,
      Phone = phone,
      Email = email,
      IsActive = isActive,
      Description = description,
      TenantId = appTenantId,
      CreatedAt = DateTime.UtcNow
    };

    branch.AddDomainEvent(new BranchCreatedEvent(branch));

    return branch;
  }

  public void Update(
      string name,
      string code,
      string? address,
      string? phone,
      string? email,
      bool isActive,
      string? description)
  {
    ArgumentException.ThrowIfNullOrEmpty(name);
    ArgumentException.ThrowIfNullOrEmpty(code);

    Name = name;
    Code = code;
    Address = address;
    Phone = phone;
    Email = email;
    IsActive = isActive;
    Description = description;
    UpdatedAt = DateTime.UtcNow;

    AddDomainEvent(new BranchUpdatedEvent(this));
  }

  public void Activate()
  {
    if (IsActive)
      throw new InvalidOperationException("Branch is already active.");

    IsActive = true;
    UpdatedAt = DateTime.UtcNow;
    AddDomainEvent(new BranchActivatedEvent(this));
  }

  public void Deactivate()
  {
    if (!IsActive)
      throw new InvalidOperationException("Branch is already inactive.");

    IsActive = false;
    UpdatedAt = DateTime.UtcNow;
    IsDeleted = true;
    AddDomainEvent(new BranchDeactivatedEvent(this));
  }

}