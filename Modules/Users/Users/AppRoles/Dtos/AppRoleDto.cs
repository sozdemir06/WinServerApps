using Users.AppRoles.Models;

namespace Users.AppRoles.Dtos;

public record AppRoleDto(
    Guid Id,
    string Name,
    string NormalizedName,
    string? Description,
    RoleLanguageKey? RoleLanguageKey,
    bool? IsActive);