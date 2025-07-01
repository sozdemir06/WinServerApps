using Shared.Exceptions;

namespace Customers.Countries.Exceptions;

public class CountryHasCitiesException : BadRequestException
{
  public CountryHasCitiesException(string message) : base(message)
  {
  }
}