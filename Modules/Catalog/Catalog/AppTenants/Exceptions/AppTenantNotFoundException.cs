using Shared.Exceptions;

namespace Catalog.AppTenants.Exceptions;

public class AppTenantNotFoundException : NotFoundException
{
  public AppTenantNotFoundException(string message) : base(message) { }


}