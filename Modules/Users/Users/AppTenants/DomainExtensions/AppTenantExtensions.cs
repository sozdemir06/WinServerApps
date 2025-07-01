using Users.AppTenants.Dtos;

namespace Users.AppTenants.DomainExtensions;

public static class AppTenantExtensions
{
  public static AppTenantDto ProjectAppTenantToDto(this AppTenant tenant)
  {
    return new AppTenantDto(
        tenant.Id,
        tenant.Name,
        tenant.Host,
        tenant.Phone,
        tenant.TenantCode,
        tenant.IsEnabledWebUi,
        tenant.Description ?? string.Empty,
        tenant.AllowedBranchNumber,
        tenant.AdminEmail!,
        tenant.IsActive,
        tenant.SubscriptionEndDate,
        tenant.SubscriptionStartDate,
        tenant.TenantType!,
        tenant.MaxUserCount,
        tenant.CreatedBy,
        tenant.ModifiedBy,
        tenant.CreatedAt,
        tenant.UpdatedAt
    );
  }

  public static IQueryable<AppTenantDto> ProjectAppTenantListToDto(this IQueryable<AppTenant> query)
  {
    return query.Select(tenant => new AppTenantDto(
        tenant.Id,
        tenant.Name,
        tenant.Host,
        tenant.Phone,
        tenant.TenantCode,
        tenant.IsEnabledWebUi,
        tenant.Description ?? string.Empty,
        tenant.AllowedBranchNumber,
        tenant.AdminEmail!,
        tenant.IsActive,
        tenant.SubscriptionEndDate,
        tenant.SubscriptionStartDate,
        tenant.TenantType!,
        tenant.MaxUserCount,
        tenant.CreatedBy,
        tenant.ModifiedBy,
        tenant.CreatedAt,
        tenant.UpdatedAt
    ));
  }

  public static IQueryable<AppTenant> FilterBySearchTerm(this IQueryable<AppTenant> query, string? searchTerm)
  {
    if (string.IsNullOrWhiteSpace(searchTerm))
      return query;

    var trimmedSearchTerm = searchTerm.Trim().ToLower();

    return query.Where(tenant =>
        tenant.Name.ToLower().Contains(trimmedSearchTerm) ||
        tenant.TenantCode.ToLower().Contains(trimmedSearchTerm) ||
        (tenant.Description != null && tenant.Description.ToLower().Contains(trimmedSearchTerm))
    );
  }
}