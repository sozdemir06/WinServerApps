using Shared.Exceptions;

namespace Users.UserRoles.Exceptions;

public class UserRoleAlreadyExistsException : BadRequestException
{
  public UserRoleAlreadyExistsException(string message) : base(message)
  {
  }
}