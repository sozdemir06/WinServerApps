using Customers.Currencies.Exceptions;


namespace Customers.Currencies.Features.DeleteCurrency;

public record DeleteCurrencyCommand(long Id) : ICommand<DeleteCurrencyResult>, ICacheRemovingRequest
{
  public List<string> CacheKeysToRemove => [CacheKeys.Currencies];
}

public record DeleteCurrencyResult;

public class DeleteCurrencyCommandValidator : AbstractValidator<DeleteCurrencyCommand>
{
  public DeleteCurrencyCommandValidator()
  {
    RuleFor(x => x.Id).GreaterThan(0).WithMessage("Id is required");
  }
}

public class DeleteCurrencyHandler(CustomerDbContext dbContext, ILocalizationService localizationService) : ICommandHandler<DeleteCurrencyCommand, DeleteCurrencyResult>
{
  public async Task<DeleteCurrencyResult> Handle(DeleteCurrencyCommand request, CancellationToken cancellationToken)
  {
    var currency = await dbContext.Currencies
        .FirstOrDefaultAsync(c => c.Id == request.Id, cancellationToken);

    if (currency == null)
      throw new CurrencyNotFoundException(await localizationService.Translate("NotFound"));

    currency.IsDeleted = true;
    await dbContext.SaveChangesAsync(cancellationToken);
    return new DeleteCurrencyResult();
  }
}