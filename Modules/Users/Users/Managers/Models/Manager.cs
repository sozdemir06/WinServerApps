using Users.Managers.DomainEvents;
using Users.UserRoles.Models;
using WinApps.Modules.Users.Users.Branches.Models;

namespace Users.Managers.Models;

public class Manager : Aggregate<Guid>
{
  public string FirstName { get; private set; } = default!;
  public string LastName { get; private set; } = default!;
  public string NormalizedUserName { get; private set; } = default!;
  public string NormalizedEmail { get; private set; } = default!;
  public string Email { get; private set; } = default!;
  public byte[] PasswordHash { get; private set; } = default!;
  public byte[] PasswordSalt { get; private set; } = default!;
  public string? PhoneNumber { get; private set; } = default!;
  public string? PhotoUrl { get; private set; } = default!;
  public string UserName { get; private set; } = default!;
  public bool EmailConfirmed { get; private set; } = default!;
  public string SecurityStamp { get; private set; } = default!;
  public bool IsAdmin { get; private set; }
  public bool IsManager { get; private set; }
  public bool IsActive { get; private set; }
  public ICollection<UserRole> UserRoles { get; private set; } = [];
  public AppTenant? Tenant { get; private set; } = default!;
  public Guid? TenantId { get; private set; } = default!;
  public Branch? Branch { get; private set; } = default!;
  public Guid? BranchId { get; private set; } = default!;


  // Private constructor for EF Core
  private Manager() { }

  public static Manager Create(
      string userName,
      string email,
      string phoneNumber,
      string firstName,
      string lastName,
      string photoUrl,
      string normalizedUserName,
      string normalizedEmail,
      bool isAdmin,
      bool isManager,
      byte[] passwordHash,
      byte[] passwordSalt,
      Guid? tenantId = null,
      Guid? branchId = null)
  {
    ArgumentException.ThrowIfNullOrEmpty(userName);
    ArgumentException.ThrowIfNullOrEmpty(email);

    var manager = new Manager
    {
      Id = Guid.CreateVersion7(),
      CreatedAt = DateTime.UtcNow,
      UserName = userName,
      NormalizedUserName = normalizedUserName,
      NormalizedEmail = normalizedEmail,
      FirstName = firstName,
      LastName = lastName,
      PhotoUrl = photoUrl,
      Email = email,
      PhoneNumber = phoneNumber,
      IsAdmin = isAdmin,
      IsManager = isManager,
      IsActive = true,
      TenantId = tenantId,
      PasswordHash = passwordHash,
      PasswordSalt = passwordSalt,
      EmailConfirmed = false,
      SecurityStamp = Guid.NewGuid().ToString(),
      BranchId = branchId
    };

    manager.AddDomainEvent(new ManagerCreatedEvent(manager));
    return manager;
  }

  public void Update(
      string userName,
      string email,
      string phoneNumber,
      bool isManager,
      Guid? tenantId,
      Guid? branchId)
  {
    ArgumentException.ThrowIfNullOrEmpty(userName);
    ArgumentException.ThrowIfNullOrEmpty(email);

    UserName = userName;
    Email = email;
    PhoneNumber = phoneNumber;
    IsManager = isManager;
    TenantId = tenantId;
    BranchId = branchId;

    AddDomainEvent(new ManagerUpdatedEvent(this));
  }

  public void Activate()
  {
    if (IsActive) return;

    IsActive = true;

    AddDomainEvent(new ManagerActivatedEvent(this));
  }

  public void Deactivate()
  {
    if (!IsActive) return;

    IsActive = false;

    AddDomainEvent(new ManagerDeactivatedEvent(this));
  }

  public void Delete() 
  {
    if (IsDeleted) return;

    IsDeleted = true;

    AddDomainEvent(new ManagerDeletedEvent(this));
  }


}
