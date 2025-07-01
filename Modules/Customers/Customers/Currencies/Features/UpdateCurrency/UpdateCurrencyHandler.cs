using Customers.Currencies.Dtos;
using Customers.Currencies.Exceptions;
using Customers.Currencies.Models;
using FluentValidation;
using Microsoft.EntityFrameworkCore;
using Shared.Constants;
using Shared.CQRS;

namespace Customers.Currencies.Features.UpdateCurrency;

public record UpdateCurrencyCommand(long Id, CurrencyDto Currency) : ICommand<UpdateCurrencyResult>, ICacheRemovingRequest
{
  public List<string> CacheKeysToRemove => [CacheKeys.Currencies];
}

public record UpdateCurrencyResult(long Id);

public class UpdateCurrencyCommandValidator : AbstractValidator<UpdateCurrencyCommand>
{
  public UpdateCurrencyCommandValidator()
  {
    RuleFor(x => x.Id).GreaterThan(0);
    RuleFor(x => x.Currency.CurrencyCode).NotEmpty().WithMessage("Currency code is required");
    RuleFor(x => x.Currency.CurrencyName).NotEmpty().WithMessage("Currency name is required");
  }
}

public class UpdateCurrencyHandler(
  CustomerDbContext dbContext,
  ILocalizationService localizationService
  ) : ICommandHandler<UpdateCurrencyCommand, UpdateCurrencyResult>
{
  public async Task<UpdateCurrencyResult> Handle(UpdateCurrencyCommand request, CancellationToken cancellationToken)
  {
    var currency = await dbContext.Currencies
        .FirstOrDefaultAsync(c => c.Id == request.Id, cancellationToken);

    if (currency == null)
      throw new CurrencyNotFoundException(await localizationService.Translate("NotFound"));

    currency.Update(
        request.Currency.CurrencyCode ?? string.Empty,
        request.Currency.CurrencyName ?? string.Empty,
        request.Currency.ForexBuying,
        request.Currency.ForexSelling,
        request.Currency.BanknoteBuying,
        request.Currency.BanknoteSelling
    );

    await dbContext.SaveChangesAsync(cancellationToken);
    return new UpdateCurrencyResult(currency.Id);
  }
}