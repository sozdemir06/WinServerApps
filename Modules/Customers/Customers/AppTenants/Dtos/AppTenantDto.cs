namespace Customers.AppTenants.Dtos;

public record AppTenantDto(
  Guid Id,
  string Name,
  string? Host,
  string? Phone,
  string TenantCode,
  bool IsEnabledWebUi,
  string? Description,
  string AdminEmail,
  int AllowedBranchNumber,
  bool IsActive,
  DateTime SubscriptionEndDate,
  DateTime? SubscriptionStartDate,
  string TenantType,
  int MaxUserCount,
  Guid ModifiedBy,
  string? CreatedBy,
  DateTime CreatedAt
);