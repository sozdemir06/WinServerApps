using Shared.Exceptions;

namespace Catalog.AppUnits.Exceptions;

public class AppUnitValidationException : BadRequestException
{
  public AppUnitValidationException(string message) : base(message)
  {
  }
}