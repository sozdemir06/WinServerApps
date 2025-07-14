using Accounting.ExpensePens.Dtos;
using Accounting.ExpensePens.Exceptions;
using Accounting.ExpensePens.Models;

namespace Accounting.ExpensePens.Features.AdminExpensePens.CreateAdminExpensePen;

public record CreateAdminExpensePenCommand(ExpensePenDto ExpensePen) : ICommand<CreateAdminExpensePenResult>, ICacheRemovingRequest
{
  public List<string> CacheKeysToRemove => [CacheKeys.AdminExpensePens];
}

public record CreateAdminExpensePenResult(Guid Id);

public class CreateAdminExpensePenCommandValidator : AbstractValidator<CreateAdminExpensePenCommand>
{
  public CreateAdminExpensePenCommandValidator()
  {
    RuleFor(x => x.ExpensePen)
        .NotNull()
        .WithMessage("ExpensePen data is required.");

    RuleFor(x => x.ExpensePen.Translates)
        .NotEmpty()
        .WithMessage("At least one translation is required.");
  }
}

public class CreateAdminExpensePenHandler : ICommandHandler<CreateAdminExpensePenCommand, CreateAdminExpensePenResult>
{
  private readonly AccountingDbContext _context;

  public CreateAdminExpensePenHandler(AccountingDbContext context)
  {
    _context = context;
  }

  public async Task<CreateAdminExpensePenResult> Handle(CreateAdminExpensePenCommand request, CancellationToken cancellationToken)
  {
    if (request.ExpensePen.Translates.Count == 0)
    {
      throw new ExpensePenBadRequestException("At least one translation is required.");
    }

    // If the new ExpensePen is being set as default, unset any existing default ExpensePen
    if (request.ExpensePen.IsDefault)
    {
      var existingDefault = await _context.ExpensePens
          .Where(ep => ep.IsDefault && !ep.IsDeleted)
          .FirstOrDefaultAsync(cancellationToken);

      if (existingDefault != null)
      {
        existingDefault.UnsetAsDefault();
      }
    }

    var expensePen = ExpensePen.Create(request.ExpensePen.IsActive);

    // Set as default if requested
    if (request.ExpensePen.IsDefault)
    {
      expensePen.SetAsDefault();
    }

    // Validate translations
    var validTranslations = request.ExpensePen.Translates
        .Where(t => !string.IsNullOrWhiteSpace(t.Name) && t.LanguageId.HasValue)
        .ToList();

    // Check for existing ExpensePens with the same translation names
    var existingExpensePenNames = validTranslations.Select(t => t.Name).ToList();

    var existingExpensePens = await _context.ExpensePens
        .Where(ep => ep.ExpensePenTranslates.Any(t => existingExpensePenNames.Contains(t.Name)))
        .AsNoTracking()
        .ToListAsync(cancellationToken);

    if (existingExpensePens.Any())
    {
      throw new ExpensePenBadRequestException("An ExpensePen with one of the provided names already exists.");
    }

    await _context.ExpensePens.AddAsync(expensePen, cancellationToken);

    // Add translations
    foreach (var transDto in validTranslations)
    {
      var newTranslate = ExpensePenTranslate.Create(transDto.Name, transDto.Description, transDto.LanguageId, expensePen.Id);
      _context.ExpensePenTranslates.Add(newTranslate);
    }

    await _context.SaveChangesAsync(cancellationToken);

    return new CreateAdminExpensePenResult(expensePen.Id);
  }
}