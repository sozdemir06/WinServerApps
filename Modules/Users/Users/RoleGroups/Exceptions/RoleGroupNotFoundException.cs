using Shared.DDD;
using Shared.Exceptions;

namespace Users.RoleGroups.Exceptions;

public class RoleGroupNotFoundException : NotFoundException
{
  public RoleGroupNotFoundException(string message, Guid id) : base(message)
  {
    Id = id;
  }

  public Guid Id { get; }
}