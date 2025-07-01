using Customers.Currencies.Dtos;
using Customers.Currencies.Exceptions;
using Mapster;
using Microsoft.EntityFrameworkCore;
using Shared.CQRS;

namespace Customers.Currencies.Features.GetCurrencyById;

public record GetCurrencyByIdQuery(long Id) : IQuery<GetCurrencyByIdResult>;

public record GetCurrencyByIdResult(CurrencyDto Currency);

public class GetCurrencyByIdHandler(CustomerDbContext dbContext, ILocalizationService localizationService) : IQueryHandler<GetCurrencyByIdQuery, GetCurrencyByIdResult>
{
  public async Task<GetCurrencyByIdResult> Handle(GetCurrencyByIdQuery request, CancellationToken cancellationToken)
  {
    var currency = await dbContext.Currencies
        .FirstOrDefaultAsync(c => c.Id == request.Id, cancellationToken);

    if (currency == null)
      throw new CurrencyNotFoundException(await localizationService.Translate("NotFound"));

    var currencyDto = currency.Adapt<CurrencyDto>();

    return new GetCurrencyByIdResult(currencyDto);
  }
}