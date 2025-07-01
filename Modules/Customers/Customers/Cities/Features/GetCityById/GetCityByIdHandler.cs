using Customers.Cities.Exceptions;

namespace Customers.Cities.Features.GetCityById;

public record GetCityByIdQuery(long Id) : IQuery<CityDto>, ICachableRequest
{
  public string CacheKey => CacheKeyGenerator.GenerateKey(CacheKeys.City, Id);
  public string CacheGroupKey => CacheKeys.Cities;
  public TimeSpan? CacheExpiration => null;
}

public class GetCityByIdHandler(CustomerDbContext dbContext, ILocalizationService localizationService)
    : IQueryHandler<GetCityByIdQuery, CityDto>
{
  public async Task<CityDto> Handle(GetCityByIdQuery request, CancellationToken cancellationToken)
  {
    var city = await dbContext.Cities
        .Include(c => c.Country)
        .AsNoTracking()
        .FirstOrDefaultAsync(c => c.Id == request.Id, cancellationToken);

    if (city == null)
    {
      throw new CityNotFoundException(await localizationService.Translate("NotFound"));
    }

    return city.Adapt<CityDto>();
  }
}