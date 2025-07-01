using Shared.DDD;
using Shared.Exceptions;

namespace WinApps.Modules.Users.Users.Branches.Exceptions;

public class BranchNotFoundException : NotFoundException
{
  public BranchNotFoundException(string message, Guid id) : base(message)
  {
    Id = id;
  }

  public Guid Id { get; }
}