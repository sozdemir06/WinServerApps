using Customers.Cities.Models;
using Customers.Cities.QueryParams;
using Shared.Pagination;

namespace Customers.Cities.Features.GetCities;

public record GetCitiesQuery(CityParams CityParams) : IQuery<GetCitiesResult>, ICachableRequest
{
  public string CacheKey => CacheKeyGenerator.GenerateKey(CacheKeys.Cities, CityParams);
  public string CacheGroupKey => CacheKeys.Cities;
  public TimeSpan? CacheExpiration => TimeSpan.FromMinutes(30);

}

public record GetCitiesResult(IEnumerable<CityDto> Cities, PaginationMetaData MetaData);

public class GetCitiesHandler : IQueryHandler<GetCitiesQuery, GetCitiesResult>
{
  private readonly CustomerDbContext _dbContext;

  public GetCitiesHandler(CustomerDbContext dbContext)
  {
    _dbContext = dbContext;
  }

  public async Task<GetCitiesResult> Handle(GetCitiesQuery request, CancellationToken cancellationToken)
  {
    var query = _dbContext.Cities
        .OrderBy(c => c.Name)
        .Where(c => c.CountryId == request.CityParams.CountryId)
        .Include(c => c.Country)
        .AsNoTracking()
        .AsQueryable();

    // Apply search if search term is provided
    if (!string.IsNullOrWhiteSpace(request.CityParams.Search))
    {
      var searchTerm = request.CityParams.Search.ToLower();
      query = query.Where(c =>
          c.Name!.ToLower().Contains(searchTerm) ||
          c.Country!.Name!.ToLower().Contains(searchTerm));
    }

    var cities = await PagedList<City>.ToPagedList(
        query,
        request.CityParams.PageNumber, 
        request.CityParams.PageSize,
        cancellationToken);

    var cityDtos = cities.Select(city => city.Adapt<CityDto>());

    return new GetCitiesResult(cityDtos, cities.MetaData);
  }
}