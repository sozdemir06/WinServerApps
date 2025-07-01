using Shared.Exceptions;

namespace WinApps.Modules.Users.Users.Managers.Exceptions;

public class LoginFailedException : BadRequestException
{
  public LoginFailedException(string message) : base($"Login failed: {message}")
  {
  }
}