using Shared.Exceptions;

namespace Accounting.TaxGroups.Exceptions;

public class TaxGroupBadRequestException : BadRequestException
{
  public TaxGroupBadRequestException(string message) : base(message)
  {
  }
}