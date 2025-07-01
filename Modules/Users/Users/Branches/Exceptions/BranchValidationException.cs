using Shared.Exceptions;

namespace WinApps.Modules.Users.Users.Branches.Exceptions;

public class BranchValidationException : BadRequestException
{
  public BranchValidationException(string message) : base(message)
  {
  }

  public BranchValidationException(string message, string details) : base(message, details)
  {
  }
}