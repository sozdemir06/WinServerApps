using Shared.Exceptions;

namespace Users.RoleGroups.Exceptions;

public class RoleGroupValidationException : BadRequestException
{
  public RoleGroupValidationException(string message) : base(message)
  {
  }
}