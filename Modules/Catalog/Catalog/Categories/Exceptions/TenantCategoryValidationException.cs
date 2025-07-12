using Shared.Exceptions;

namespace WinApps.Modules.Catalog.Catalog.Categories.Exceptions;

public class TenantCategoryValidationException : BadRequestException
{
  public TenantCategoryValidationException(string message) : base(message)
  {
  }
}