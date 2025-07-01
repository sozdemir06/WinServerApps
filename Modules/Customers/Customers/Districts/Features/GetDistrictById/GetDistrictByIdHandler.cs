using Customers.Districts.Exceptions;


namespace Customers.Districts.Features.GetDistrictById;

public record GetDistrictByIdQuery(long Id) : IQuery<DistrictDto>, ICachableRequest
{
  public string CacheKey => CacheKeyGenerator.GenerateKey(CacheKeys.District, Id);
  public string CacheGroupKey => CacheKeys.Districts;
  public TimeSpan? CacheExpiration => TimeSpan.FromMinutes(30);
}

public class GetDistrictByIdHandler(CustomerDbContext dbContext, ILocalizationService localizationService)
    : IQueryHandler<GetDistrictByIdQuery, DistrictDto>
{
  public async Task<DistrictDto> Handle(GetDistrictByIdQuery request, CancellationToken cancellationToken)
  {
    var district = await dbContext.Districts
        .Include(d => d.City)
        .Include(d => d.Country)
        .AsNoTracking()
        .FirstOrDefaultAsync(d => d.Id == request.Id, cancellationToken)
        ?? throw new DistrictNotFoundException(await localizationService.Translate("NotFound"));

    return district.Adapt<DistrictDto>();
  }
}