using Shared.Exceptions;

namespace Accounting.TaxGroups.Exceptions;

public class TaxGroupNotFoundException : NotFoundException
{
  public TaxGroupNotFoundException(Guid id) : base($"TaxGroup with id '{id}' was not found.")
  {
  }

  public TaxGroupNotFoundException(string message) : base(message)
  {
  }
}