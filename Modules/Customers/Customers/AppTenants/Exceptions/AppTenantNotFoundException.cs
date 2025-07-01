using Shared.Exceptions;

namespace Customers.AppTenants.Exceptions;

public class AppTenantNotFoundException : NotFoundException
{
  public AppTenantNotFoundException(string message) : base(message) { }

  
}