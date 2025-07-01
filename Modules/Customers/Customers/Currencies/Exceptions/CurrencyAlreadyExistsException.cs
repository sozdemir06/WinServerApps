using Shared.DDD;
using Shared.Exceptions;

namespace Customers.Currencies.Exceptions;

public class CurrencyAlreadyExistsException : BadRequestException
{
  public CurrencyAlreadyExistsException(string message)
      : base(message)
  {
  }
}