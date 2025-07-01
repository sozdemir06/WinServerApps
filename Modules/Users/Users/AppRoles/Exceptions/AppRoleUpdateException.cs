using Shared.Exceptions;

namespace Users.AppRoles.Exceptions;

public class AppRoleUpdateException : BadRequestException
{
  public AppRoleUpdateException(string message) : base(message)
  {
  }

  public AppRoleUpdateException(string message, string details) : base(message, details)
  {
  }
}