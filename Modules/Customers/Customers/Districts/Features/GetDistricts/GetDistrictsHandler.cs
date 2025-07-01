using Customers.Districts.Models;
using Customers.Districts.QueryParams;
using Shared.Pagination;

namespace Customers.Districts.Features.GetDistricts;

public record GetDistrictsQuery(DistrictParams DistrictParams) : IQuery<GetDistrictsResult>, ICachableRequest
{
  public string CacheKey => CacheKeyGenerator.GenerateKey(CacheKeys.Districts, DistrictParams);
  public string CacheGroupKey => CacheKeys.Districts;
  public TimeSpan? CacheExpiration => null;
}

public record GetDistrictsResult(IEnumerable<DistrictDto> Districts, PaginationMetaData MetaData);

public class GetDistrictsValidator : AbstractValidator<GetDistrictsQuery>
{
  public GetDistrictsValidator()
  {
    

    RuleFor(x => x.DistrictParams.CityId)
        .GreaterThan(0)
        .When(x => x.DistrictParams.CityId.HasValue)
        .WithMessage("City ID must be greater than 0");


  }

}

public class GetDistrictsHandler(CustomerDbContext dbContext) : IQueryHandler<GetDistrictsQuery, GetDistrictsResult>
{
  public async Task<GetDistrictsResult> Handle(GetDistrictsQuery request, CancellationToken cancellationToken)
  {
    var query = dbContext.Districts
        .OrderBy(d => d.Name)
        .Where(d => d.CityId == request.DistrictParams.CityId)
        .AsNoTracking()
        .AsQueryable();

    // Apply search if search term is provided
    if (!string.IsNullOrWhiteSpace(request.DistrictParams.SearchTerm))
    {
      var searchTerm = request.DistrictParams.SearchTerm.ToLower();
      query = query.Where(d =>
          d.Name!.ToLower().Contains(searchTerm) ||
          d.City!.Name!.ToLower().Contains(searchTerm) ||
          d.Country!.Name!.ToLower().Contains(searchTerm));
    }


    var districts = await PagedList<District>.ToPagedList(
        query,
        request.DistrictParams.PageNumber,
        request.DistrictParams.PageSize,
        cancellationToken);

    var districtDtos = districts.Select(district => district.Adapt<DistrictDto>());

    return new GetDistrictsResult(districtDtos, districts.MetaData);
  }
}