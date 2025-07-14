using Shared.Exceptions;

namespace Catalog.AppUnits.Exceptions;

public class AppUnitBadRequestException : BadRequestException
{
  public AppUnitBadRequestException(string message) : base(message)
  {
  }
}