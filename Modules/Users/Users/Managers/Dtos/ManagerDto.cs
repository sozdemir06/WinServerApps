using Users.UserRoles.Dtos;

namespace Users.Managers.Dtos;

public record ManagerDto(
    Guid Id,
    string FirstName,
    string LastName,
    string UserName,
    string Email,
    string? PhoneNumber,
    string? PhotoUrl,
    bool IsAdmin,
    bool IsManager,
    bool IsActive,
    Guid? TenantId,
    Guid? BranchId,
    DateTime CreatedAt,
    string? Password = null,
    string? BranchName = null,
    string? TenantName = null,
    List<UserRoleDto>? UserRoles = null
    );

