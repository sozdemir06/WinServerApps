using Shared.Exceptions;

namespace Users.RoleGroups.Exceptions;

public class RoleGroupBadRequestException : BadRequestException
{
  public RoleGroupBadRequestException(string message) : base(message)
  {
  }
}