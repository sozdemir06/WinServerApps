using Shared.Exceptions;

namespace Users.AppRoles.Exceptions;

public class AppRoleAlreadyExistsException : BadRequestException
{
  public AppRoleAlreadyExistsException(string roleName)
      : base($"A role with name '{roleName}' already exists.")
  {
  }
}