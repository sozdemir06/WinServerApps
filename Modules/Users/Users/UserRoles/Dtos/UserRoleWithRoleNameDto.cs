

using Users.AppRoles.Models;

namespace Users.UserRoles.Dtos
{
    public record UserRoleWithRoleNameDto(string RoleName,string RoleNormalizedName,string? RoleDescription,RoleLanguageKey? RoleLanguageKey,bool IsActive);
  
}