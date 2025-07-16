using Shared.Exceptions;

namespace Accounting.Taxes.Exceptions;

public class TaxInternalServerException : InternalServerErrorException
{
  public TaxInternalServerException(string message) : base(message) { }
}