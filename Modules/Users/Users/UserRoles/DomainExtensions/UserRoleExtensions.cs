using Users.AppRoles.Dtos;
using Users.AppRoles.Models;
using Users.UserRoles.Dtos;
using Users.UserRoles.Models;

namespace Users.UserRoles.DomainExtensions;

public static class UserRoleExtensions
{
  public static UserRoleDto ProjectUserRoleToDto(this UserRole userRole)
  {
    return new UserRoleDto(
        userRole.Id,
        userRole.ManagerId,
        userRole.RoleId,
        userRole.IsActive
    );
  }

  public static IQueryable<UserRoleDto> ProjectUserRoleListToDto(this IQueryable<UserRole> query)
  {
    return query.Select(userRole => new UserRoleDto(
        userRole.Id,
        userRole.ManagerId,
        userRole.RoleId,
        userRole.IsActive
      
    ));
  }

  public static IQueryable<UserRoleWithRoleNameDto> ProjectUserRoleListToDtoWithRole(this IQueryable<UserRole> query)
  {
    return query.Select(userRole => new UserRoleWithRoleNameDto(
        userRole.AppRole.Name,
        userRole.AppRole.NormalizedName,
        userRole.AppRole.Description!,
        userRole.AppRole.RoleLanguageKey!.Value,
        userRole.AppRole.IsActive
      ));
  }
}