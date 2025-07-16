using Shared.Exceptions;

namespace Accounting.Taxes.Exceptions;

public class TaxBadRequestException : BadRequestException
{
    public TaxBadRequestException(string message) : base(message) { }
}