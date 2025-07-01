using Users.Managers.Models;
using WinApps.Modules.Users.Users.Branches.Models;

namespace Users.AppTenants.Models;

public class AppTenant : Aggregate<Guid>
{
  public string Name { get; private set; } = default!;
  public string? Host { get; private set; }
  public string? Phone { get; private set; }
  public string TenantCode { get; private set; } = default!;
  public bool IsEnabledWebUi { get; private set; } = default!;
  public string? Description { get; private set; }
  public int AllowedBranchNumber { get; private set; } = default!;

  public bool IsActive { get; private set; } = default!;
  public string? AdminEmail { get; private set; } = default!;
  public DateTime SubscriptionEndDate { get; private set; } = default!;
  public DateTime? SubscriptionStartDate { get; private set; } = default!;
  public string? TenantType { get; private set; } = default!;
  public int MaxUserCount { get; private set; } = default!;

  //Navigation Properties
  public ICollection<Manager> Managers { get; private set; } = default!;
  public ICollection<Branch> Branches { get; private set; } = default!;

  // Private constructor for EF Core
  private AppTenant() { }

  public static AppTenant Create(
    string name,
    string? host,    
    string? phone,
    string tenantCode,
    bool isEnabledWebUi,
    string description,
    string adminEmail,
    int allowedBranchNumber,
    bool isActive,
    DateTime subscriptionEndDate,
    DateTime? subscriptionStartDate,
    string tenantType,
    int maxUserCount)
  {
    ArgumentException.ThrowIfNullOrEmpty(name);
    ArgumentException.ThrowIfNullOrEmpty(adminEmail);

    var tenant = new AppTenant
    {
      Id = Guid.CreateVersion7(),
      Name = name,
      Host = host,
      Phone = phone,
      Description = description,
      IsEnabledWebUi = isEnabledWebUi,
      IsActive = isActive,
      TenantCode = tenantCode,
      AdminEmail = adminEmail,
      MaxUserCount = maxUserCount,
      AllowedBranchNumber = allowedBranchNumber,
      SubscriptionStartDate = subscriptionStartDate,
      SubscriptionEndDate = subscriptionEndDate,
      TenantType = tenantType,
      CreatedAt = DateTime.UtcNow
    };

    tenant.AddDomainEvent(new AppTenantCreatedEvent(tenant));

    return tenant;
  }

  public void Update(
    string name,
    string? host,
    string? phone,
    string tenantCode,
    bool isEnabledWebUi,
    string description,
    string adminEmail,
    int allowedBranchNumber,
    bool isActive,
    DateTime subscriptionEndDate,
    DateTime? subscriptionStartDate,
    string tenantType,
    int maxUserCount)
  {
    ArgumentException.ThrowIfNullOrEmpty(name);
    ArgumentException.ThrowIfNullOrEmpty(tenantCode);

    Name = name;
    Host = host;
    Phone = phone;
    TenantCode = tenantCode;
    IsEnabledWebUi = isEnabledWebUi;
    Description = description;
    AdminEmail = adminEmail;
    AllowedBranchNumber = allowedBranchNumber;
    IsActive = isActive;
    SubscriptionEndDate = subscriptionEndDate;
    SubscriptionStartDate = subscriptionStartDate;
    TenantType = tenantType;
    MaxUserCount = maxUserCount;

    //AddDomainEvent(new AppTenantUpdatedEvent(this));
  }

  public void UpdateSubscription(DateTime startDate, DateTime endDate, Guid modifiedBy)
  {
    if (endDate <= startDate)
    {
      throw new ArgumentException("End date must be after start date");
    }

    SubscriptionStartDate = startDate;
    SubscriptionEndDate = endDate;
  }

  public void Activate()
  {
    IsActive = true;
  }

  public void Deactivate()
  {
    IsActive = false;
  }

  public void ToggleWebUi(bool isEnabled)
  {
    IsEnabledWebUi = isEnabled;
  }
}