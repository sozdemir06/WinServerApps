using Shared.Exceptions;

namespace Users.AppRoles.Exceptions;

public class AppRoleValidationException : BadRequestException
{
  public AppRoleValidationException(string message) : base(message)
  {
  }

}