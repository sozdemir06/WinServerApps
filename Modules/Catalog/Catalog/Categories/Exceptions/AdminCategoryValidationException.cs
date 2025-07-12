using Shared.Exceptions;

namespace WinApps.Modules.Catalog.Catalog.Categories.Exceptions;

public class AdminCategoryValidationException : BadRequestException
{
  public AdminCategoryValidationException(string message) : base(message)
  {
  }
}