using Accounting.TaxGroups.Exceptions;
using Accounting.TaxGroups.Models;

namespace Accounting.TaxGroups.Features.TenantTaxGroups.DeleteTenantTaxGroup;

public record DeleteTenantTaxGroupCommand(Guid Id) : ICommand<DeleteTenantTaxGroupResult>, ICacheRemovingRequest
{
  public List<string> CacheKeysToRemove => [CacheKeys.TenantTaxGroups];
}

public record DeleteTenantTaxGroupResult(Guid Id);

public class DeleteTenantTaxGroupCommandValidator : AbstractValidator<DeleteTenantTaxGroupCommand>
{
  public DeleteTenantTaxGroupCommandValidator()
  {
    RuleFor(x => x.Id)
        .NotEmpty()
        .WithMessage("TenantTaxGroup ID is required.");
  }
}

public class DeleteTenantTaxGroupHandler : ICommandHandler<DeleteTenantTaxGroupCommand, DeleteTenantTaxGroupResult>
{
  private readonly AccountingDbContext _context;

  public DeleteTenantTaxGroupHandler(AccountingDbContext context)
  {
    _context = context;
  }

  public async Task<DeleteTenantTaxGroupResult> Handle(DeleteTenantTaxGroupCommand request, CancellationToken cancellationToken)
  {
    var tenantTaxGroup = await _context.TenantTaxGroups
        .FirstOrDefaultAsync(ttg => ttg.Id == request.Id, cancellationToken);

    if (tenantTaxGroup == null)
    {
      throw new TaxGroupNotFoundException(request.Id);
    }

    // Soft delete the TenantTaxGroup
    tenantTaxGroup.Deactivate();

    await _context.SaveChangesAsync(cancellationToken);

    return new DeleteTenantTaxGroupResult(tenantTaxGroup.Id);
  }
}