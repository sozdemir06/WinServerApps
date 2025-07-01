

using Shared.Exceptions;

namespace Users.AppTenants.Exceptions;

public class AppTenantNotFoundException : NotFoundException
{
  public AppTenantNotFoundException(string message) : base(message)
  {
  }

  public AppTenantNotFoundException(string name, object key) : base(name, key)
  {
  }
}
