using Shared.DDD;

namespace Customers.AppTenants.Models;

public class AppTenant : Entity<Guid>
{
  public string Name { get; private set; } = default!;
  public string? Host { get; private set; } = default!;
  public string? Phone { get; private set; } = default!;
  public string TenantCode { get; private set; } = default!;
  public bool IsEnabledWebUi { get; private set; }
  public string? Description { get; private set; } = default!;
  public int AllowedBranchNumber { get; private set; } = default!;
  public bool IsActive { get; private set; } = default!;
  public string AdminEmail { get; private set; } = default!;
  public DateTime SubscriptionEndDate { get; private set; } = default!;
  public DateTime? SubscriptionStartDate { get; private set; } = default!;
  public string TenantType { get; private set; } = default!;
  public int MaxUserCount { get; private set; } = default!;

  private AppTenant() { } // For EF Core

  public static AppTenant Create(
      Guid id,
      string name,
      string? host,
      string? phone,
      string tenantCode,
      bool isEnabledWebUi,
      string? description,
      string adminEmail,
      int allowedBranchNumber,
      bool isActive,
      DateTime subscriptionEndDate,
      DateTime? subscriptionStartDate,
      string tenantType,
      int maxUserCount)
  {
    return new AppTenant
    {
      Id = id,
      Name = name,
      Host = host,
      Phone = phone,
      TenantCode = tenantCode,
      IsEnabledWebUi = isEnabledWebUi,
      Description = description,
      AdminEmail = adminEmail,
      AllowedBranchNumber = allowedBranchNumber,
      IsActive = isActive,
      SubscriptionEndDate = subscriptionEndDate,
      SubscriptionStartDate = subscriptionStartDate,
      TenantType = tenantType,
      MaxUserCount = maxUserCount,
      CreatedAt = DateTime.UtcNow
    };
  }

  public void Update(
      string name,
      string? host,
      string? phone,
      string tenantCode,
      bool isEnabledWebUi,
      string? description,
      string adminEmail,
      int allowedBranchNumber,
      bool isActive,
      DateTime subscriptionEndDate,
      DateTime? subscriptionStartDate,
      string tenantType,
      int maxUserCount)
  {
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
  }

  public void Deactivate(Guid modifiedBy)
  {
    IsActive = false;
  }

  public void Activate(Guid modifiedBy)
  {
    IsActive = true;
  }
}