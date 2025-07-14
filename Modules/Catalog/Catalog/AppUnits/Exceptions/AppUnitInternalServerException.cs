using Shared.Exceptions;

namespace Catalog.AppUnits.Exceptions;

public class AppUnitInternalServerException : InternalServerErrorException
{
  public AppUnitInternalServerException(string message) : base(message)
  {
  }
}