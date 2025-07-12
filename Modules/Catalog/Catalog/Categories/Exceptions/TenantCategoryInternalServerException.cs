using Shared.Exceptions;

namespace WinApps.Modules.Catalog.Catalog.Categories.Exceptions;

public class TenantCategoryInternalServerException : InternalServerErrorException
{
  public TenantCategoryInternalServerException(string message) : base(message)
  {
  }
}