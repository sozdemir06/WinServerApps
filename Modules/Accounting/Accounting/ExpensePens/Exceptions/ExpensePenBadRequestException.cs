using Shared.Exceptions;

namespace Accounting.ExpensePens.Exceptions;

public class ExpensePenBadRequestException : BadRequestException
{
  public ExpensePenBadRequestException(string message) : base(message)
  {
  }
}