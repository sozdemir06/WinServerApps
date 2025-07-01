namespace Users.AppTenants.Dtos;

/// <summary>
/// Data Transfer Object for AppTenant entity
/// </summary>
public record AppTenantDto
(
  Guid Id,
  string Name,
  string? Host,
  string? Phone,
  string TenantCode,
  bool IsEnabledWebUi,
  string Description,
  int AllowedBranchNumber,
  string AdminEmail,
  bool IsActive,
  DateTime SubscriptionEndDate,
  DateTime? SubscriptionStartDate,
  string TenantType,
  int MaxUserCount,
  string? CreatedBy,
  string? ModifiedBy,
  DateTime CreatedAt,
  DateTime? UpdatedAt
);


