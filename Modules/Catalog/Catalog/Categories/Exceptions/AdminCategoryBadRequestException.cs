using Shared.Exceptions;

namespace WinApps.Modules.Catalog.Catalog.Categories.Exceptions;

public class AdminCategoryBadRequestException : BadRequestException
{
  public AdminCategoryBadRequestException(string message) : base(message)
  {
  }
}