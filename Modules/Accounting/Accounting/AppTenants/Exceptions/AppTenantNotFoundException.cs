using Shared.Exceptions;

namespace Accounting.AppTenants.Exceptions;

public class AppTenantNotFoundException : NotFoundException
{
  public AppTenantNotFoundException(string message) : base(message) { }
}