using Shared.Exceptions;

namespace Users.AppRoles.Exceptions;

public class AppRoleNotFoundException : NotFoundException
{
  public AppRoleNotFoundException(Guid roleId)
      : base($"AppRole with ID {roleId} was not found.")
  {
  }
}