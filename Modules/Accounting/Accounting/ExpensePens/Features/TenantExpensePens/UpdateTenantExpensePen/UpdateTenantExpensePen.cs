using Accounting.ExpensePens.Dtos;
using Accounting.ExpensePens.Exceptions;
using Accounting.ExpensePens.Models;

namespace Accounting.ExpensePens.Features.TenantExpensePens.UpdateTenantExpensePen;

public record UpdateTenantExpensePenCommand(Guid Id, Guid TenantId, ExpensePenDto ExpensePen) : ICommand<UpdateTenantExpensePenResult>, ICacheRemovingRequest
{
  public List<string> CacheKeysToRemove => [CacheKeys.TenantExpensePens];
}

public record UpdateTenantExpensePenResult(Guid Id);

public class UpdateTenantExpensePenCommandValidator : AbstractValidator<UpdateTenantExpensePenCommand>
{
  public UpdateTenantExpensePenCommandValidator()
  {
    RuleFor(x => x.ExpensePen)
        .NotNull()
        .WithMessage("ExpensePen data is required.");

    RuleFor(x => x.TenantId)
        .NotEmpty()
        .WithMessage("TenantId is required.");

    RuleFor(x => x.ExpensePen.Translates)
        .NotEmpty()
        .WithMessage("At least one translation is required.");
  }
}

public class UpdateTenantExpensePenHandler : ICommandHandler<UpdateTenantExpensePenCommand, UpdateTenantExpensePenResult>
{
  private readonly AccountingDbContext _context;

  public UpdateTenantExpensePenHandler(AccountingDbContext context)
  {
    _context = context;
  }

  public async Task<UpdateTenantExpensePenResult> Handle(UpdateTenantExpensePenCommand request, CancellationToken cancellationToken)
  {
    if (request.ExpensePen.Translates.Count == 0)
    {
      throw new ExpensePenBadRequestException("At least one translation is required.");
    }

    var tenantExpensePen = await _context.TenantExpensePens
        .Include(tep => tep.TenantExpensePenTranslates)
        .FirstOrDefaultAsync(tep => tep.Id == request.Id && tep.TenantId == request.TenantId && !tep.IsDeleted, cancellationToken);

    if (tenantExpensePen == null)
    {
      throw new ExpensePenNotFoundException(request.Id);
    }

    // If the TenantExpensePen is being set as default, unset any existing default TenantExpensePen for this tenant
    if (request.ExpensePen.IsDefault && !tenantExpensePen.IsDefault)
    {
      var existingDefault = await _context.TenantExpensePens
          .Where(tep => tep.TenantId == request.TenantId && tep.IsDefault && !tep.IsDeleted && tep.Id != request.Id)
          .FirstOrDefaultAsync(cancellationToken);

      if (existingDefault != null)
      {
        existingDefault.UnsetAsDefault();
      }
    }

    tenantExpensePen.Update(request.ExpensePen.IsActive);

    // Set or unset as default
    if (request.ExpensePen.IsDefault && !tenantExpensePen.IsDefault)
    {
      tenantExpensePen.SetAsDefault();
    }
    else if (!request.ExpensePen.IsDefault && tenantExpensePen.IsDefault)
    {
      tenantExpensePen.UnsetAsDefault();
    }

    // Validate translations
    var validTranslations = request.ExpensePen.Translates
        .Where(t => !string.IsNullOrWhiteSpace(t.Name) && t.LanguageId.HasValue)
        .ToList();

    // Check for existing TenantExpensePens with the same translation names for this tenant (excluding current TenantExpensePen)
    var existingTenantExpensePenNames = validTranslations.Select(t => t.Name).ToList();

    var existingTenantExpensePens = await _context.TenantExpensePens
        .Where(tep => tep.TenantId == request.TenantId && tep.Id != request.Id &&
                     tep.TenantExpensePenTranslates.Any(t => existingTenantExpensePenNames.Contains(t.Name)))
        .AsNoTracking()
        .ToListAsync(cancellationToken);

    if (existingTenantExpensePens.Any())
    {
      throw new ExpensePenBadRequestException("A TenantExpensePen with one of the provided names already exists for this tenant.");
    }

    // Remove existing translations from database
    _context.TenantExpensePenTranslates.RemoveRange(tenantExpensePen.TenantExpensePenTranslates);

    // Add new translations
    foreach (var transDto in validTranslations)
    {
      var newTranslate = TenantExpensePenTranslate.Create(transDto.Name, transDto.Description, transDto.LanguageId, tenantExpensePen.Id);
      _context.TenantExpensePenTranslates.Add(newTranslate);
    }

    await _context.SaveChangesAsync(cancellationToken);

    return new UpdateTenantExpensePenResult(tenantExpensePen.Id);
  }
}