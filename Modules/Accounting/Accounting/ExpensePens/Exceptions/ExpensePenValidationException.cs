using Shared.Exceptions;

namespace Accounting.ExpensePens.Exceptions;

public class ExpensePenValidationException : BadRequestException
{
  public ExpensePenValidationException(string message) : base(message)
  {
  }
}