using Shared.Exceptions;

namespace WinApps.Modules.Catalog.Catalog.Categories.Exceptions;

public class AdminCategoryInternalServerException : InternalServerErrorException
{
  public AdminCategoryInternalServerException(string message) : base(message)
  {
  }
}