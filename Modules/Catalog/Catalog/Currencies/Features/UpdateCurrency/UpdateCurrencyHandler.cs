using Catalog.Currencies.Dtos;
using Catalog.Currencies.Models;
using Catalog.Data;

namespace Catalog.Currencies.Features.UpdateCurrency;

public record UpdateCurrencyCommand(CurrencyDto Currency) : ICommand<UpdateCurrencyResult>;

public record UpdateCurrencyResult(bool Success);

public class UpdateCurrencyCommandValidator : AbstractValidator<UpdateCurrencyCommand>
{
  public UpdateCurrencyCommandValidator()
  {
    RuleFor(x => x.Currency.Id).NotEmpty().WithMessage("Id is required");
    RuleFor(x => x.Currency.CurrencyCode).NotEmpty().WithMessage("CurrencyCode is required");
    RuleFor(x => x.Currency.CurrencyName).NotEmpty().WithMessage("CurrencyName is required");
  }
}

public class UpdateCurrencyHandler(CatalogDbContext dbContext) : ICommandHandler<UpdateCurrencyCommand, UpdateCurrencyResult>
{
  public async Task<UpdateCurrencyResult> Handle(UpdateCurrencyCommand request, CancellationToken cancellationToken)
  {
    var currency = await dbContext.Currencies
        .FirstOrDefaultAsync(x => x.Id == request.Currency.Id || x.CurrencyCode == request.Currency.CurrencyCode, cancellationToken);

    if (currency == null)
    {
      // Create new currency if not exists
      currency = Currency.Create(
          request.Currency.Id,
          request.Currency.CurrencyCode!,
          request.Currency.CurrencyName!,
          request.Currency.ForexBuying,
          request.Currency.ForexSelling,
          request.Currency.BanknoteBuying,
          request.Currency.BanknoteSelling);

      await dbContext.Currencies.AddAsync(currency, cancellationToken);
    }
    else
    {
      // Update existing currency
      currency.Update(
          request.Currency.CurrencyCode!,
          request.Currency.CurrencyName!,
          request.Currency.ForexBuying,
          request.Currency.ForexSelling,
          request.Currency.BanknoteBuying,
          request.Currency.BanknoteSelling);
    }

    await dbContext.SaveChangesAsync(cancellationToken);
    return new UpdateCurrencyResult(true);
  }
}