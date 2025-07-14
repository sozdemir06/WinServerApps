using Accounting.ExpensePens.Dtos;
using Accounting.ExpensePens.Exceptions;
using Accounting.ExpensePens.Models;
using FluentValidation;

namespace Accounting.ExpensePens.Features.AdminExpensePens.UpdateAdminExpensePen;

public record UpdateAdminExpensePenCommand(Guid Id, ExpensePenDto ExpensePen) : ICommand<UpdateAdminExpensePenResult>, ICacheRemovingRequest
{
  public List<string> CacheKeysToRemove => [CacheKeys.AdminExpensePens];
}

public record UpdateAdminExpensePenResult(Guid Id);

public class UpdateAdminExpensePenCommandValidator : AbstractValidator<UpdateAdminExpensePenCommand>
{
  public UpdateAdminExpensePenCommandValidator()
  {
    RuleFor(x => x.ExpensePen)
        .NotNull()
        .WithMessage("ExpensePen data is required.");

    RuleFor(x => x.ExpensePen.Translates)
        .NotEmpty()
        .WithMessage("At least one translation is required.");
  }
}

public class UpdateAdminExpensePenHandler : ICommandHandler<UpdateAdminExpensePenCommand, UpdateAdminExpensePenResult>
{
  private readonly AccountingDbContext _context;

  public UpdateAdminExpensePenHandler(AccountingDbContext context)
  {
    _context = context;
  }

  public async Task<UpdateAdminExpensePenResult> Handle(UpdateAdminExpensePenCommand request, CancellationToken cancellationToken)
  {
    if (request.ExpensePen.Translates.Count == 0)
    {
      throw new ExpensePenBadRequestException("At least one translation is required.");
    }

    var expensePen = await _context.ExpensePens
        .Include(ep => ep.ExpensePenTranslates)
        .FirstOrDefaultAsync(ep => ep.Id == request.Id && !ep.IsDeleted, cancellationToken);

    if (expensePen == null)
    {
      throw new ExpensePenNotFoundException(request.Id);
    }

    // If the ExpensePen is being set as default, unset any existing default ExpensePen
    if (request.ExpensePen.IsDefault && !expensePen.IsDefault)
    {
      var existingDefault = await _context.ExpensePens
          .Where(ep => ep.IsDefault && !ep.IsDeleted && ep.Id != request.Id)
          .FirstOrDefaultAsync(cancellationToken);

      if (existingDefault != null)
      {
        existingDefault.UnsetAsDefault();
      }
    }

    expensePen.Update(request.ExpensePen.IsActive);

    // Set or unset as default
    if (request.ExpensePen.IsDefault && !expensePen.IsDefault)
    {
      expensePen.SetAsDefault();
    }
    else if (!request.ExpensePen.IsDefault && expensePen.IsDefault)
    {
      expensePen.UnsetAsDefault();
    }

    // Validate translations
    var validTranslations = request.ExpensePen.Translates
        .Where(t => !string.IsNullOrWhiteSpace(t.Name) && t.LanguageId.HasValue)
        .ToList();

    // Check for existing ExpensePens with the same translation names (excluding current ExpensePen)
    var existingExpensePenNames = validTranslations.Select(t => t.Name).ToList();

    var existingExpensePens = await _context.ExpensePens
        .Where(ep => ep.Id != request.Id && ep.ExpensePenTranslates.Any(t => existingExpensePenNames.Contains(t.Name)))
        .AsNoTracking()
        .ToListAsync(cancellationToken);

    if (existingExpensePens.Any())
    {
      throw new ExpensePenBadRequestException("An ExpensePen with one of the provided names already exists.");
    }

    // Remove existing translations from database
    _context.ExpensePenTranslates.RemoveRange(expensePen.ExpensePenTranslates);

    // Add new translations
    foreach (var transDto in validTranslations)
    {
      var newTranslate = ExpensePenTranslate.Create(transDto.Name, transDto.Description, transDto.LanguageId, expensePen.Id);
      _context.ExpensePenTranslates.Add(newTranslate);
    }

    await _context.SaveChangesAsync(cancellationToken);

    return new UpdateAdminExpensePenResult(expensePen.Id);
  }
}