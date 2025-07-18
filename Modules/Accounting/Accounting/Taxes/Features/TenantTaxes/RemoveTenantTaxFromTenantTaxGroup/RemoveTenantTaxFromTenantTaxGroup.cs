using Accounting.Taxes.Exceptions;
using Accounting.Taxes.Models;
using Accounting.TaxGroups.Exceptions;

namespace Accounting.Taxes.Features.TenantTaxes.RemoveTenantTaxFromTenantTaxGroup;

public record RemoveTenantTaxFromTenantTaxGroupCommand(Guid TenantTaxGroupId, Guid TenantTaxId) : ICommand<RemoveTenantTaxFromTenantTaxGroupResult>, ICacheRemovingRequest
{
  public List<string> CacheKeysToRemove => [CacheKeys.TenantTaxes, CacheKeys.TenantTaxGroups];
}

public record RemoveTenantTaxFromTenantTaxGroupResult(Guid TenantTaxId, Guid TenantTaxGroupId);

public class RemoveTenantTaxFromTenantTaxGroupCommandValidator : AbstractValidator<RemoveTenantTaxFromTenantTaxGroupCommand>
{
  public RemoveTenantTaxFromTenantTaxGroupCommandValidator()
  {
    RuleFor(x => x.TenantTaxGroupId)
        .NotEmpty()
        .WithMessage("TenantTaxGroup ID is required.");

    RuleFor(x => x.TenantTaxId)
        .NotEmpty()
        .WithMessage("TenantTax ID is required.");
  }
}

public class RemoveTenantTaxFromTenantTaxGroupHandler : ICommandHandler<RemoveTenantTaxFromTenantTaxGroupCommand, RemoveTenantTaxFromTenantTaxGroupResult>
{
  private readonly AccountingDbContext _context;

  public RemoveTenantTaxFromTenantTaxGroupHandler(AccountingDbContext context)
  {
    _context = context;
  }

  public async Task<RemoveTenantTaxFromTenantTaxGroupResult> Handle(RemoveTenantTaxFromTenantTaxGroupCommand request, CancellationToken cancellationToken)
  {
    // Verify the tenant tax group exists
    var tenantTaxGroup = await _context.TenantTaxGroups
        .FirstOrDefaultAsync(ttg => ttg.Id == request.TenantTaxGroupId, cancellationToken);

    if (tenantTaxGroup == null)
    {
      throw new TaxGroupNotFoundException(request.TenantTaxGroupId);
    }

    // Find the tenant tax and verify it belongs to the specified tenant tax group
    var tenantTax = await _context.TenantTaxes
        .FirstOrDefaultAsync(tt => tt.Id == request.TenantTaxId && tt.TenantTaxGroupId == request.TenantTaxGroupId, cancellationToken);

    if (tenantTax == null)
    {
      throw new TaxNotFoundException(request.TenantTaxId);
    }

    // Soft delete the tenant tax
    tenantTax.Deactivate();

    await _context.SaveChangesAsync(cancellationToken);

    return new RemoveTenantTaxFromTenantTaxGroupResult(tenantTax.Id, request.TenantTaxGroupId);
  }
}