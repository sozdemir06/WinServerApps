using Shared.Exceptions;

namespace Accounting.TaxGroups.Exceptions;

public class TaxGroupInternalServerException : InternalServerErrorException
{
  public TaxGroupInternalServerException(string message) : base(message)
  {
  }
}