using Shared.Exceptions;

namespace WinApps.Modules.Users.Users.Managers.Exceptions;

public class InvalidPasswordException : BadRequestException
{
  public InvalidPasswordException() : base("Invalid password.")
  {
  }
}