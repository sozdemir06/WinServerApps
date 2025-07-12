namespace Shared.Messages.Events.Users.AppTennats;

public record AppTenantCreatedIntegrationEvent : IntegrationEvent
{
                public Guid Id { get; set; }
                public string Name { get; set; } = default!;
                public string? Host { get; set; }
                public string? Phone { get; set; }
                public string TenantCode { get; set; } = default!;
                public bool IsEnabledWebUi { get; set; } = default!;
                public string? Description { get; set; }
                public int AllowedBranchNumber { get; set; } = default!;
                public bool IsActive { get; set; } = default!;
                public string AdminEmail { get; set; } = default!;
                public DateTime SubscriptionEndDate { get; set; } = default!;
                public DateTime? SubscriptionStartDate { get; set; }
                public string TenantType { get; set; } = default!;
                public int MaxUserCount { get; set; } = default!;
                public string? CreatedBy { get; set; }
                public DateTime CreatedAt { get; set; } = default!;
}
