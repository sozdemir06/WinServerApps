namespace Users.UserRoles.Dtos;

public record UserRoleDto(
    Guid Id,
    Guid ManagerId,
    Guid RoleId,
    bool IsActive
);