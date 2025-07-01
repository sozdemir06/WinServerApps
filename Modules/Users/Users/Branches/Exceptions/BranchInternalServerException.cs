using Shared.Exceptions;

namespace WinApps.Modules.Users.Users.Branches.Exceptions;

public class BranchInternalServerException : InternalServerErrorException
{
  public BranchInternalServerException(string message) : base(message)
  {
  }

  public BranchInternalServerException(string message, string details) : base(message, details)
  {
  }
}