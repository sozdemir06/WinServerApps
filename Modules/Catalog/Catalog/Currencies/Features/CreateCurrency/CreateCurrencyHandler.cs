using Catalog.Currencies.Dtos;
using Catalog.Currencies.Exceptions;
using Catalog.Currencies.Models;
using Catalog.Data;

namespace Catalog.Currencies.Features.CreateCurrency;

public record CreateCurrencyCommand(CurrencyDto Currency) : ICommand<CreateCurrencyResult>;

public record CreateCurrencyResult(bool Success);

public class CreateCurrencyCommandValidator : AbstractValidator<CreateCurrencyCommand>
{
  public CreateCurrencyCommandValidator()
  {
    RuleFor(x => x.Currency.CurrencyCode).NotEmpty().WithMessage("CurrencyCode is required");
    RuleFor(x => x.Currency.CurrencyName).NotEmpty().WithMessage("CurrencyName is required");
  }
}

public class CreateCurrencyHandler(CatalogDbContext dbContext, ILocalizationService localizationService) : ICommandHandler<CreateCurrencyCommand, CreateCurrencyResult>
{
  public async Task<CreateCurrencyResult> Handle(CreateCurrencyCommand request, CancellationToken cancellationToken)
  {
    var existingCurrency = await dbContext.Currencies
        .FirstOrDefaultAsync(x => x.CurrencyCode == request.Currency.CurrencyCode, cancellationToken);

    if (existingCurrency != null)
    {
      throw new CurrencyNotFoundException(await localizationService.Translate("AlreadyExistsMessage"));
    }

    var currency = CreateNewCurrency(request.Currency);
    await dbContext.Currencies.AddAsync(currency, cancellationToken);
    await dbContext.SaveChangesAsync(cancellationToken);

    return new CreateCurrencyResult(true);
  }

  private Currency CreateNewCurrency(CurrencyDto currencyDto)
  {
    var currency = Currency.Create(
        currencyDto.Id,
        currencyDto.CurrencyCode!,
        currencyDto.CurrencyName!,
        currencyDto.ForexBuying,
        currencyDto.ForexSelling,
        currencyDto.BanknoteBuying,
        currencyDto.BanknoteSelling);

    return currency;
  }
}