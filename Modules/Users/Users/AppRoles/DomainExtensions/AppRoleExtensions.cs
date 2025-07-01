using Users.AppRoles.Dtos;
using Users.AppRoles.Models;

namespace Users.AppRoles.DomainExtensions;

public static class AppRoleExtensions
{
  public static AppRoleDto ProjectAppRoleToDto(this AppRole role)
  {
    return new AppRoleDto(
        role.Id,
        role.Name,
        role.NormalizedName,
        role.Description ?? string.Empty,
        role.RoleLanguageKey,
        role.IsActive
    );
  }

  public static IQueryable<AppRoleDto> ProjectAppRoleListToDto(this IQueryable<AppRole> query)
  {
    return query.Select(role => new AppRoleDto(
        role.Id,
        role.Name,
        role.NormalizedName,
        role.Description ?? string.Empty,
        role.RoleLanguageKey,
        role.IsActive
    ));
  }
}