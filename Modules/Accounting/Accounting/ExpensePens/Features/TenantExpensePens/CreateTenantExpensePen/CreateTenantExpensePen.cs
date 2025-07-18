using Accounting.ExpensePens.Dtos;
using Accounting.ExpensePens.Exceptions;
using Accounting.ExpensePens.Models;

namespace Accounting.ExpensePens.Features.TenantExpensePens.CreateTenantExpensePen;

public record CreateTenantExpensePenCommand(ExpensePenDto ExpensePen, Guid? TenantId) : ICommand<CreateTenantExpensePenResult>, ICacheRemovingRequest
{
  public List<string> CacheKeysToRemove => [CacheKeys.TenantExpensePens];
}

public record CreateTenantExpensePenResult(Guid Id);

public class CreateTenantExpensePenCommandValidator : AbstractValidator<CreateTenantExpensePenCommand>
{
  public CreateTenantExpensePenCommandValidator()
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

public class CreateTenantExpensePenHandler : ICommandHandler<CreateTenantExpensePenCommand, CreateTenantExpensePenResult>
{
  private readonly AccountingDbContext _context;

  public CreateTenantExpensePenHandler(AccountingDbContext context)
  {
    _context = context;
  }

  public async Task<CreateTenantExpensePenResult> Handle(CreateTenantExpensePenCommand request, CancellationToken cancellationToken)
  {
    if (request.ExpensePen.Translates.Count == 0)
    {
      throw new ExpensePenBadRequestException("At least one translation is required.");
    }

    // If the new TenantExpensePen is being set as default, unset any existing default TenantExpensePen for this tenant
    if (request.ExpensePen.IsDefault)
    {
      var existingDefault = await _context.TenantExpensePens
          .Where(tep => tep.IsDefault)
          .FirstOrDefaultAsync(cancellationToken);

      if (existingDefault != null)
      {
        existingDefault.UnsetAsDefault();
      }
    }

    var tenantExpensePen = TenantExpensePen.Create(request.TenantId!, request.ExpensePen.IsActive);

    // Set as default if requested
    if (request.ExpensePen.IsDefault)
    {
      tenantExpensePen.SetAsDefault();
    }

    // Validate translations
    var validTranslations = request.ExpensePen.Translates
        .Where(t => !string.IsNullOrWhiteSpace(t.Name) && t.LanguageId.HasValue)
        .ToList();

    // Check for existing TenantExpensePens with the same translation names for this tenant
    var existingTenantExpensePenNames = validTranslations.Select(t => t.Name).ToList();

    var existingTenantExpensePens = await _context.TenantExpensePens
        .Where(tep => tep.TenantId == request.TenantId &&
                     tep.TenantExpensePenTranslates.Any(t => existingTenantExpensePenNames.Contains(t.Name)))
        .AsNoTracking()
        .ToListAsync(cancellationToken);

    if (existingTenantExpensePens.Any())
    {
      throw new ExpensePenBadRequestException("A TenantExpensePen with one of the provided names already exists for this tenant.");
    }

    await _context.TenantExpensePens.AddAsync(tenantExpensePen, cancellationToken);

    // Add translations
    foreach (var transDto in validTranslations)
    {
      var newTranslate = TenantExpensePenTranslate.Create(transDto.Name, transDto.Description, transDto.LanguageId, tenantExpensePen.Id);
      _context.TenantExpensePenTranslates.Add(newTranslate);
    }

    await _context.SaveChangesAsync(cancellationToken);

    return new CreateTenantExpensePenResult(tenantExpensePen.Id);
  }
}