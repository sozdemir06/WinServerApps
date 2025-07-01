using Shared.Exceptions;

namespace Users.UserRoles.Exceptions;

public class UserRoleNotFoundException : NotFoundException
{
  public UserRoleNotFoundException(string message) : base(message)
  {
  }
}