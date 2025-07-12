namespace WinApps.Modules.Users.Users.Branches.Dtos;

public record BranchDto(
    Guid Id,
    string Name,
    string Code,
    string? Address,
    string? Phone,
    string? Email,
    bool IsActive,
    string? Description,
    Guid TenantId,
    string? CreatedBy,
    string? ModifiedBy,
    DateTime CreatedAt,
    DateTime? UpdatedAt,
    AppTenantDto AppTenant
    
);