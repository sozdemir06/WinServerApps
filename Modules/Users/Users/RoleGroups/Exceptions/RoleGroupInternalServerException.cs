using Shared.Exceptions;

namespace Users.RoleGroups.Exceptions;

public class RoleGroupInternalServerException : InternalServerErrorException
{
  public RoleGroupInternalServerException(string message) : base(message)
  {
  }
}