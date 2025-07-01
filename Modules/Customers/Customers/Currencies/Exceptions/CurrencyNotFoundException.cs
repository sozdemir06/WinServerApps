using Shared.DDD;
using Shared.Exceptions;

namespace Customers.Currencies.Exceptions;

public class CurrencyNotFoundException : NotFoundException
{
  public CurrencyNotFoundException(string message)
      : base(message)
  {
  }
}