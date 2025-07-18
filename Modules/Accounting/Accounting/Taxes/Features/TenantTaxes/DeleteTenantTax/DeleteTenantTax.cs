using Accounting.Taxes.Exceptions;
using Accounting.Taxes.Models;

namespace Accounting.Taxes.Features.TenantTaxes.DeleteTenantTax;

public record DeleteTenantTaxCommand(Guid Id) : ICommand<DeleteTenantTaxResult>, ICacheRemovingRequest
{
  public List<string> CacheKeysToRemove => [CacheKeys.TenantTaxes];
}

public record DeleteTenantTaxResult(Guid Id);

public class DeleteTenantTaxCommandValidator : AbstractValidator<DeleteTenantTaxCommand>
{
  public DeleteTenantTaxCommandValidator()
  {
    RuleFor(x => x.Id)
        .NotEmpty()
        .WithMessage("TenantTax ID is required.");
  }
}

public class DeleteTenantTaxHandler : ICommandHandler<DeleteTenantTaxCommand, DeleteTenantTaxResult>
{
  private readonly AccountingDbContext _context;

  public DeleteTenantTaxHandler(AccountingDbContext context)
  {
    _context = context;
  }

  public async Task<DeleteTenantTaxResult> Handle(DeleteTenantTaxCommand request, CancellationToken cancellationToken)
  {
    var tenantTax = await _context.TenantTaxes
        .FirstOrDefaultAsync(tt => tt.Id == request.Id, cancellationToken);

    if (tenantTax == null)
    {
      throw new TaxNotFoundException(request.Id);
    }

    // Soft delete the TenantTax
    tenantTax.Deactivate();

    await _context.SaveChangesAsync(cancellationToken);

    return new DeleteTenantTaxResult(tenantTax.Id);
  }
}