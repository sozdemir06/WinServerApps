using Customers.Countries.QueryParams;
using Shared.Pagination;


namespace Customers.Countries.Features.GetCountries;

public record GetCountriesQuery(CountryParams CountryParams) : IQuery<GetCountriesResult>, ICachableRequest
{
  public string CacheKey => CacheKeyGenerator.GenerateKey(CacheKeys.Countries, CountryParams);
  public string CacheGroupKey => CacheKeys.Countries;
  public TimeSpan? CacheExpiration => null;
}

public record GetCountriesResult(IEnumerable<CountryDto> Countries, PaginationMetaData MetaData);

public class GetCountriesHandler : IQueryHandler<GetCountriesQuery, GetCountriesResult>
{
  private readonly CustomerDbContext _dbContext;

  public GetCountriesHandler(CustomerDbContext dbContext)
  {
    _dbContext = dbContext;
  }

  public async Task<GetCountriesResult> Handle(GetCountriesQuery request, CancellationToken cancellationToken)
  {
    var query = _dbContext.Countries
        .OrderBy(c => c.Name)
        .Include(c => c.Cities)
        .AsNoTracking()
        .AsQueryable();

    // Apply search if search term is provided
    if (!string.IsNullOrWhiteSpace(request.CountryParams.Search))
    {
      var searchTerm = request.CountryParams.Search.ToLower();
      query = query.Where(c =>
          c.Name!.ToLower().Contains(searchTerm) ||
          c.CountryCode!.ToLower().Contains(searchTerm) ||
          c.Iso2!.ToLower().Contains(searchTerm) ||
          c.Iso3!.ToLower().Contains(searchTerm));
    }


    var countries = await PagedList<Country>.ToPagedList(
        query,
        request.CountryParams.PageNumber,
        request.CountryParams.PageSize,
        cancellationToken);

    var countryDtos = countries.Select(country => country.Adapt<CountryDto>());

    return new GetCountriesResult(countryDtos, countries.MetaData);
  }
}