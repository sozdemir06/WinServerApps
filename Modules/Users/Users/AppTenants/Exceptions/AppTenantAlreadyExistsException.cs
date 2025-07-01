using Shared.Exceptions;

namespace WinApps.Modules.Users.Users.AppTenants.Exceptions;

public class AppTenantAlreadyExistsException : BadRequestException
{
  public AppTenantAlreadyExistsException(string tenantCode)
      : base($"App tenant with code '{tenantCode}' already exists.")
  {
  }
}