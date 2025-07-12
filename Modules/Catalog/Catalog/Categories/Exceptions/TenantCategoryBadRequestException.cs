using Shared.Exceptions;

namespace WinApps.Modules.Catalog.Catalog.Categories.Exceptions;

public class TenantCategoryBadRequestException : BadRequestException
{
  public TenantCategoryBadRequestException(string message) : base(message)
  {
  }
}