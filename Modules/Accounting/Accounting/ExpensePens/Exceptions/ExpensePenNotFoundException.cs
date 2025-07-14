using Shared.Exceptions;

namespace Accounting.ExpensePens.Exceptions;

public class ExpensePenNotFoundException : NotFoundException
{
  public ExpensePenNotFoundException(string message) : base(message)
  {
  }

  public ExpensePenNotFoundException(Guid id) : base($"ExpensePen with id {id} not found.")
  {
  }
}