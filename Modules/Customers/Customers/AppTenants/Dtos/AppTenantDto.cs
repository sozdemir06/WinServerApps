namespace Customers.AppTenants.Dtos;

public class AppTenantDto
{
  public Guid Id { get; set; }
  public string Name { get; set; } = default!;
  public string? Host { get; set; }
  public string? Phone { get; set; }
  public string TenantCode { get; set; } = default!;
  public bool IsEnabledWebUi { get; set; }
  public string? Description { get; set; }
  public string AdminEmail { get; set; } = default!;
  public int AllowedBranchNumber { get; set; }
  public bool IsActive { get; set; }
  public DateTime SubscriptionEndDate { get; set; }
  public DateTime? SubscriptionStartDate { get; set; }
  public string TenantType { get; set; } = default!;
  public int MaxUserCount { get; set; }
  public Guid ModifiedBy { get; set; }
  public string? CreatedBy { get; set; }
  public DateTime CreatedAt { get; set; }
}