using Shared.Exceptions;

namespace Accounting.ExpensePens.Exceptions;

public class ExpensePenInternalServerException : InternalServerErrorException
{
  public ExpensePenInternalServerException(string message) : base(message)
  {
  }
}