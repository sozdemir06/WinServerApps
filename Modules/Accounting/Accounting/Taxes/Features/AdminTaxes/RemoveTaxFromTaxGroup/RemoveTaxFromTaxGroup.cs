using Accounting.Taxes.Exceptions;
using Accounting.Taxes.Models;
using Accounting.TaxGroups.Exceptions;

namespace Accounting.Taxes.Features.AdminTaxes.RemoveTaxFromTaxGroup;

public record RemoveTaxFromTaxGroupCommand(Guid TaxGroupId, Guid TaxId) : ICommand<RemoveTaxFromTaxGroupResult>, ICacheRemovingRequest
{
  public List<string> CacheKeysToRemove => [CacheKeys.AdminTaxes, CacheKeys.AdminTaxGroups];
}

public record RemoveTaxFromTaxGroupResult(Guid TaxId, Guid TaxGroupId);

public class RemoveTaxFromTaxGroupCommandValidator : AbstractValidator<RemoveTaxFromTaxGroupCommand>
{
  public RemoveTaxFromTaxGroupCommandValidator()
  {
    RuleFor(x => x.TaxGroupId)
        .NotEmpty()
        .WithMessage("TaxGroup ID is required.");

    RuleFor(x => x.TaxId)
        .NotEmpty()
        .WithMessage("Tax ID is required.");
  }
}

public class RemoveTaxFromTaxGroupHandler : ICommandHandler<RemoveTaxFromTaxGroupCommand, RemoveTaxFromTaxGroupResult>
{
  private readonly AccountingDbContext _context;

  public RemoveTaxFromTaxGroupHandler(AccountingDbContext context)
  {
    _context = context;
  }

  public async Task<RemoveTaxFromTaxGroupResult> Handle(RemoveTaxFromTaxGroupCommand request, CancellationToken cancellationToken)
  {
    // Verify the tax group exists
    var taxGroup = await _context.TaxGroups
        .FirstOrDefaultAsync(tg => tg.Id == request.TaxGroupId, cancellationToken);

    if (taxGroup == null)
    {
      throw new TaxGroupNotFoundException(request.TaxGroupId);
    }

    // Find the tax and verify it belongs to the specified tax group
    var tax = await _context.Taxes
        .FirstOrDefaultAsync(t => t.Id == request.TaxId && t.TaxGroupId == request.TaxGroupId, cancellationToken);

    if (tax == null)
    {
      throw new TaxNotFoundException(request.TaxId);
    }

    // Soft delete the tax
    tax.Deactivate();

    await _context.SaveChangesAsync(cancellationToken);

    return new RemoveTaxFromTaxGroupResult(tax.Id, request.TaxGroupId);
  }
}