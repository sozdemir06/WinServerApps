using Shared.Exceptions;

namespace Users.UserRoles.Exceptions;

public class UserRoleValidationException : BadRequestException
{
  public UserRoleValidationException(string message) : base(message)
  {
  }
}