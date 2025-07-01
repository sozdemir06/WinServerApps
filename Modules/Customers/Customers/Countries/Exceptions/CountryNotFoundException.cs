using Shared.DDD;
using Shared.Exceptions;

namespace Customers.Countries.Exceptions;

public class CountryNotFoundException : NotFoundException
{
  public CountryNotFoundException(string message)
      : base(message)
  {
  }
}