using Accounting.TaxGroups.Exceptions;
using Accounting.TaxGroups.Models;

namespace Accounting.TaxGroups.Features.AdminTaxGroups.DeleteAdminTaxGroup;

public record DeleteAdminTaxGroupCommand(Guid Id) : ICommand<DeleteAdminTaxGroupResult>, ICacheRemovingRequest
{
  public List<string> CacheKeysToRemove => [CacheKeys.AdminTaxGroups];
}

public record DeleteAdminTaxGroupResult(Guid Id);

public class DeleteAdminTaxGroupCommandValidator : AbstractValidator<DeleteAdminTaxGroupCommand>
{
  public DeleteAdminTaxGroupCommandValidator()
  {
    RuleFor(x => x.Id)
        .NotEmpty()
        .WithMessage("TaxGroup ID is required.");
  }
}

public class DeleteAdminTaxGroupHandler : ICommandHandler<DeleteAdminTaxGroupCommand, DeleteAdminTaxGroupResult>
{
  private readonly AccountingDbContext _context;

  public DeleteAdminTaxGroupHandler(AccountingDbContext context)
  {
    _context = context;
  }

  public async Task<DeleteAdminTaxGroupResult> Handle(DeleteAdminTaxGroupCommand request, CancellationToken cancellationToken)
  {
    var taxGroup = await _context.TaxGroups
        .FirstOrDefaultAsync(tg => tg.Id == request.Id, cancellationToken);

    if (taxGroup == null)
    {
      throw new TaxGroupNotFoundException(request.Id);
    }

    // Soft delete the TaxGroup
    taxGroup.Deactivate();

    await _context.SaveChangesAsync(cancellationToken);

    return new DeleteAdminTaxGroupResult(taxGroup.Id);
  }
}