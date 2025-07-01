using Shared.Exceptions;

namespace Users.AppTenants.Exceptions;

public class AppTenantUpdateException : BadRequestException
{
  public AppTenantUpdateException(string message) : base(message)
  {
  }

  public AppTenantUpdateException(string message, string details) : base(message, details)
  {
  }
}