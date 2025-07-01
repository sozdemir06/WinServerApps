using Customers.Currencies.Dtos;
using Customers.Currencies.Models;
using Customers.Currencies.QueryParams;
using Shared.Pagination;

namespace Customers.Currencies.Features.GetCurrencies;

public record GetCurrenciesQuery(CurrencyParams CurrencyParams) : IQuery<GetCurrenciesResult>, ICachableRequest
{
  public string CacheKey => CacheKeyGenerator.GenerateKey(CacheKeys.Currencies, CurrencyParams);
  public string CacheGroupKey => CacheKeys.Currencies;
  public TimeSpan? CacheExpiration => null;
}

internal interface ICacheableRequest
{
}

public record GetCurrenciesResult(IEnumerable<CurrencyDto> Currencies, PaginationMetaData MetaData);

public class GetCurrenciesHandler(CustomerDbContext dbContext) : IQueryHandler<GetCurrenciesQuery, GetCurrenciesResult>
{

    public async Task<GetCurrenciesResult> Handle(GetCurrenciesQuery request, CancellationToken cancellationToken)
  {
    var query = dbContext.Currencies
        .OrderBy(c => c.CurrencyName)
        .AsNoTracking()
        .AsQueryable();

    // Apply search if search term is provided
    if (!string.IsNullOrWhiteSpace(request.CurrencyParams.Search))
    {
      var searchTerm = request.CurrencyParams.Search.ToLower();
      query = query.Where(c =>
          c.CurrencyName!.ToLower().Contains(searchTerm) ||
          c.CurrencyCode!.ToLower().Contains(searchTerm));
    }

    // Apply filters - since we don't have IsActive property, we'll skip this filter
    // if (request.CurrencyParams.IsActive.HasValue)
    // {
    //   query = query.Where(c => c.IsActive == request.CurrencyParams.IsActive.Value);
    // }

    // Apply sorting
    query = request.CurrencyParams.SortBy?.ToLower() switch
    {
      "name" => request.CurrencyParams.SortDescending ? query.OrderByDescending(c => c.CurrencyName) : query.OrderBy(c => c.CurrencyName),
      "code" => request.CurrencyParams.SortDescending ? query.OrderByDescending(c => c.CurrencyCode) : query.OrderBy(c => c.CurrencyCode),
      "forexbuying" => request.CurrencyParams.SortDescending ? query.OrderByDescending(c => c.ForexBuying) : query.OrderBy(c => c.ForexBuying),
      "forexselling" => request.CurrencyParams.SortDescending ? query.OrderByDescending(c => c.ForexSelling) : query.OrderBy(c => c.ForexSelling),
      _ => request.CurrencyParams.SortDescending ? query.OrderByDescending(c => c.Id) : query.OrderBy(c => c.Id)
    };

    var currencies = await PagedList<Currency>.ToPagedList(
        query,
        request.CurrencyParams.PageNumber,
        request.CurrencyParams.PageSize,
        cancellationToken);

    var currencyDtos = currencies.Select(currency => currency.Adapt<CurrencyDto>());

    return new GetCurrenciesResult(currencyDtos, currencies.MetaData);
  }
}