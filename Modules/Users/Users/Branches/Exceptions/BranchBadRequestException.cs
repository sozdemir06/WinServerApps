using Shared.Exceptions;

namespace WinApps.Modules.Users.Users.Branches.Exceptions;

public class BranchBadRequestException : BadRequestException
{
  public BranchBadRequestException(string message) : base(message)
  {
  }

  public BranchBadRequestException(string message, string details) : base(message, details)
  {
  }
}