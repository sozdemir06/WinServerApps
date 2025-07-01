using Shared.Exceptions;

namespace Customers.Cities.Exceptions;

public class CityNotFoundException : NotFoundException
{
  public CityNotFoundException(string message) : base(message)
  {
  }
}