using Shared.Exceptions;

namespace Accounting.TaxGroups.Exceptions;

public class TaxGroupValidationException : ValidationException
{
  public TaxGroupValidationException(string message) : base(message)
  {
  }
}