using Shared.Exceptions;

namespace Accounting.Currencies.Exceptions;

public class CurrencyNotFoundException : NotFoundException
{
  public CurrencyNotFoundException(string message) : base(message) { }
}