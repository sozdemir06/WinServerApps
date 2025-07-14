using Accounting.ExpensePens.Exceptions;
using Accounting.ExpensePens.Models;

namespace Accounting.ExpensePens.Features.AdminExpensePens.DeleteAdminExpensePen;

public record DeleteAdminExpensePenCommand(Guid Id) : ICommand<DeleteAdminExpensePenResult>, ICacheRemovingRequest
{
  public List<string> CacheKeysToRemove => [CacheKeys.AdminExpensePens];
}

public record DeleteAdminExpensePenResult(Guid Id);

public class DeleteAdminExpensePenHandler : ICommandHandler<DeleteAdminExpensePenCommand, DeleteAdminExpensePenResult>
{
  private readonly AccountingDbContext _context;

  public DeleteAdminExpensePenHandler(AccountingDbContext context)
  {
    _context = context;
  }

  public async Task<DeleteAdminExpensePenResult> Handle(DeleteAdminExpensePenCommand request, CancellationToken cancellationToken)
  {
    var expensePen = await _context.ExpensePens.FindAsync(request.Id);

    if (expensePen == null)
    {
      throw new ExpensePenNotFoundException(request.Id);
    }

    expensePen.Deactivate();

    await _context.SaveChangesAsync(cancellationToken);

    return new DeleteAdminExpensePenResult(expensePen.Id);
  }
}