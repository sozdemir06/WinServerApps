using Shared.Exceptions;

namespace WinApps.Modules.Users.Users.Managers.Exceptions;

public class InvalidPasswordException : BadRequestException
{
  public InvalidPasswordException(string message) : base(message)
  {
  }
}