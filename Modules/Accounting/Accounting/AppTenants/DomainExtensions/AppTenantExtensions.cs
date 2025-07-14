using Accounting.AppTenants.Dtos;

namespace Accounting.AppTenants.DomainExtensions;

public static class AppTenantExtensions
{
  public static AppTenantDto ProjectAppTenantToDto(this AppTenant tenant)
  {
    return new AppTenantDto
    {
      Id = tenant.Id,
      Name = tenant.Name,
      Host = tenant.Host,
      Phone = tenant.Phone,
      TenantCode = tenant.TenantCode,
      IsEnabledWebUi = tenant.IsEnabledWebUi,
      Description = tenant.Description,
      AdminEmail = tenant.AdminEmail,
      AllowedBranchNumber = tenant.AllowedBranchNumber,
      IsActive = tenant.IsActive,
      SubscriptionEndDate = tenant.SubscriptionEndDate,
      SubscriptionStartDate = tenant.SubscriptionStartDate,
      TenantType = tenant.TenantType ?? string.Empty,
      MaxUserCount = tenant.MaxUserCount,
      ModifiedBy = Guid.Empty, // Bu alan Accounting modülünde farklı olabilir
      CreatedBy = tenant.CreatedBy,
      CreatedAt = tenant.CreatedAt
    };
  }

  public static IQueryable<AppTenantDto> ProjectAppTenantListToDto(this IQueryable<AppTenant> query)
  {
    return query.Select(tenant => new AppTenantDto
    {
      Id = tenant.Id,
      Name = tenant.Name,
      Host = tenant.Host,
      Phone = tenant.Phone,
      TenantCode = tenant.TenantCode,
      IsEnabledWebUi = tenant.IsEnabledWebUi,
      Description = tenant.Description,
      AdminEmail = tenant.AdminEmail,
      AllowedBranchNumber = tenant.AllowedBranchNumber,
      IsActive = tenant.IsActive,
      SubscriptionEndDate = tenant.SubscriptionEndDate,
      SubscriptionStartDate = tenant.SubscriptionStartDate,
      TenantType = tenant.TenantType ?? string.Empty,
      MaxUserCount = tenant.MaxUserCount,
      ModifiedBy = Guid.Empty, // Bu alan Accounting modülünde farklı olabilir
      CreatedBy = tenant.CreatedBy,
      CreatedAt = tenant.CreatedAt
    });
  }
}