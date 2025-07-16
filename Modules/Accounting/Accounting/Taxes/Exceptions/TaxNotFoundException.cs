using Shared.Exceptions;


namespace Accounting.Taxes.Exceptions;

public class TaxNotFoundException : NotFoundException
{
  public TaxNotFoundException(Guid id) : base($"Tax with ID {id} was not found.") { }
  public TaxNotFoundException(string message) : base(message) { }
}