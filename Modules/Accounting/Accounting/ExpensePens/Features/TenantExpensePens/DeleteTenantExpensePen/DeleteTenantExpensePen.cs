using Accounting.ExpensePens.Exceptions;

namespace Accounting.ExpensePens.Features.TenantExpensePens.DeleteTenantExpensePen;

public record DeleteTenantExpensePenCommand(Guid Id, Guid TenantId) : ICommand<DeleteTenantExpensePenResult>, ICacheRemovingRequest
{
  public List<string> CacheKeysToRemove => [CacheKeys.TenantExpensePens];
}

public record DeleteTenantExpensePenResult(Guid Id);

public class DeleteTenantExpensePenHandler : ICommandHandler<DeleteTenantExpensePenCommand, DeleteTenantExpensePenResult>
{
  private readonly AccountingDbContext _context;

  public DeleteTenantExpensePenHandler(AccountingDbContext context)
  {
    _context = context;
  }

  public async Task<DeleteTenantExpensePenResult> Handle(DeleteTenantExpensePenCommand request, CancellationToken cancellationToken)
  {
    var tenantExpensePen = await _context.TenantExpensePens
        .FirstOrDefaultAsync(tep => tep.Id == request.Id && tep.TenantId == request.TenantId, cancellationToken);

    if (tenantExpensePen == null)
    {
      throw new ExpensePenNotFoundException(request.Id);
    }

    tenantExpensePen.Deactivate();

    await _context.SaveChangesAsync(cancellationToken);

    return new DeleteTenantExpensePenResult(tenantExpensePen.Id);
  }
}