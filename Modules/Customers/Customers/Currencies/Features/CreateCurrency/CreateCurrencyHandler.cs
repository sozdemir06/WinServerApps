using Customers.Currencies.Dtos;
using Customers.Currencies.Exceptions;
using Customers.Currencies.Models;


namespace Customers.Currencies.Features.CreateCurrency;

public record CreateCurrencyCommand(CurrencyDto Currency) : ICommand<CreateCurrencyResult>, ICacheRemovingRequest
{
  public List<string> CacheKeysToRemove => [CacheKeys.Currencies];
}

public record CreateCurrencyResult(long Id);

public class CreateCurrencyCommandValidator : AbstractValidator<CreateCurrencyCommand>
{
  public CreateCurrencyCommandValidator()
  {
    RuleFor(x => x.Currency.CurrencyCode).NotEmpty().WithMessage("Currency code is required");
    RuleFor(x => x.Currency.CurrencyName).NotEmpty().WithMessage("Currency name is required");
  }
}

public class CreateCurrencyHandler(CustomerDbContext dbContext) : ICommandHandler<CreateCurrencyCommand, CreateCurrencyResult>
{

  public async Task<CreateCurrencyResult> Handle(CreateCurrencyCommand request, CancellationToken cancellationToken)
  {
    // Check if currency with same code already exists
    var existingCurrency = await dbContext.Currencies
        .FirstOrDefaultAsync(c => c.CurrencyCode == request.Currency.CurrencyCode && !c.IsDeleted, cancellationToken);

    if (existingCurrency != null)
    {
      throw new CurrencyAlreadyExistsException($"Currency with code {request.Currency.CurrencyCode} already exists");
    }

    var currency = CreateNewCurrency(request.Currency);
    await dbContext.Currencies.AddAsync(currency, cancellationToken);
    await dbContext.SaveChangesAsync(cancellationToken);
    return new CreateCurrencyResult(currency.Id);
  }

  private Currency CreateNewCurrency(CurrencyDto currencyDto)
  {
    return Currency.Create(
        currencyDto.Id,
        currencyDto.CurrencyCode ?? string.Empty,
        currencyDto.CurrencyName ?? string.Empty,
        currencyDto.ForexBuying,
        currencyDto.ForexSelling,
        currencyDto.BanknoteBuying,
        currencyDto.BanknoteSelling
    );
  }
}