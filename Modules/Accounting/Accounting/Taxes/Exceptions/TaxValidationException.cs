

namespace Accounting.Taxes.Exceptions;

public class TaxValidationException : ValidationException
{
  public TaxValidationException(string message) : base(message) { }
}