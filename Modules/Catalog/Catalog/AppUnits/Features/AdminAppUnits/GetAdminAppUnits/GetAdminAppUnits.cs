using Catalog.AppUnits.DomainExtensions;
using Catalog.AppUnits.Dtos;
using Catalog.AppUnits.QueryParams;

namespace Catalog.AppUnits.Features.AdminAppUnits.GetAdminAppUnits;

public record GetAdminAppUnitsQuery(AppUnitParams Parameters) : IQuery<GetAdminAppUnitsResult>, ICachableRequest
{
  public string CacheKey => CacheKeyGenerator.GenerateKey(CacheKeys.AdminAppUnits, Parameters);
  public string CacheGroupKey => CacheKeys.AdminAppUnits;
  public TimeSpan? CacheExpiration => null;
}

public record GetAdminAppUnitsResult(IEnumerable<AppUnitDto> AppUnits, PaginationMetaData MetaData);

public class GetAdminAppUnitsHandler : IQueryHandler<GetAdminAppUnitsQuery, GetAdminAppUnitsResult>
{
  private readonly CatalogDbContext _context;

  public GetAdminAppUnitsHandler(CatalogDbContext context)
  {
    _context = context;
  }

  public async Task<GetAdminAppUnitsResult> Handle(GetAdminAppUnitsQuery request, CancellationToken cancellationToken)
  {
    var query = _context.AppUnits
        .Include(u => u.Translates)
            .ThenInclude(t => t.Language)
        .IgnoreQueryFilters()
        .Where(x => !x.IsDeleted)
        .AsNoTracking()
        .ApplyAppUnitFilters(request.Parameters)
        .ApplyAppUnitOrdering()
        .ProjectAppUnitListToDto();

    var appUnits = await PagedList<AppUnitDto>.ToPagedList(
        query,
        request.Parameters.PageNumber,
        request.Parameters.PageSize,
        cancellationToken);

    return new GetAdminAppUnitsResult(appUnits, appUnits.MetaData);
  }
}