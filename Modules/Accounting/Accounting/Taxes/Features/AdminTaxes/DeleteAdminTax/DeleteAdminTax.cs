using Accounting.Taxes.Exceptions;
using Accounting.Taxes.Models;

namespace Accounting.Taxes.Features.AdminTaxes.DeleteAdminTax;

public record DeleteAdminTaxCommand(Guid Id) : ICommand<DeleteAdminTaxResult>, ICacheRemovingRequest
{
  public List<string> CacheKeysToRemove => [CacheKeys.AdminTaxes];
}

public record DeleteAdminTaxResult(Guid Id);

public class DeleteAdminTaxCommandValidator : AbstractValidator<DeleteAdminTaxCommand>
{
  public DeleteAdminTaxCommandValidator()
  {
    RuleFor(x => x.Id)
        .NotEmpty()
        .WithMessage("Tax ID is required.");
  }
}

public class DeleteAdminTaxHandler : ICommandHandler<DeleteAdminTaxCommand, DeleteAdminTaxResult>
{
  private readonly AccountingDbContext _context;

  public DeleteAdminTaxHandler(AccountingDbContext context)
  {
    _context = context;
  }

  public async Task<DeleteAdminTaxResult> Handle(DeleteAdminTaxCommand request, CancellationToken cancellationToken)
  {
    var tax = await _context.Taxes
        .FirstOrDefaultAsync(t => t.Id == request.Id, cancellationToken);

    if (tax == null)
    {
      throw new TaxNotFoundException(request.Id);
    }

    // Soft delete the Tax
    tax.Deactivate();

    await _context.SaveChangesAsync(cancellationToken);

    return new DeleteAdminTaxResult(tax.Id);
  }
}