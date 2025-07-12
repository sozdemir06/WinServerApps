using Shared.Exceptions;

namespace Catalog.Currencies.Exceptions;

public class CurrencyNotFoundException : NotFoundException
{
  public CurrencyNotFoundException(string message) : base(message) { }
}