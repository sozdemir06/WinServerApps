using Catalog.AppUnits.DomainExtensions;
using Catalog.AppUnits.Dtos;
using Catalog.AppUnits.Exceptions;
using Shared.Services.Caching;

namespace Catalog.AppUnits.Features.AdminAppUnits.GetAdminAppUnitById;

public record GetAdminAppUnitByIdQuery(Guid Id) : IQuery<GetAdminAppUnitByIdResult>, ICachableRequest
{
  public string CacheKey => CacheKeyGenerator.GenerateKey(CacheKeys.AdminAppUnits, Id);
  public string CacheGroupKey => CacheKeys.AdminAppUnits;
  public TimeSpan? CacheExpiration => null;
}

public record GetAdminAppUnitByIdResult(AppUnitDto AppUnit);

public class GetAdminAppUnitByIdHandler : IQueryHandler<GetAdminAppUnitByIdQuery, GetAdminAppUnitByIdResult>
{
  private readonly CatalogDbContext _context;

  public GetAdminAppUnitByIdHandler(CatalogDbContext context)
  {
    _context = context;
  }

  public async Task<GetAdminAppUnitByIdResult> Handle(GetAdminAppUnitByIdQuery request, CancellationToken cancellationToken)
  {
    var appUnit = await _context.AppUnits
        .Include(u => u.Translates)
            .ThenInclude(t => t.Language)
        .IgnoreQueryFilters()
        .Where(x => !x.IsDeleted)
        .AsNoTracking()
        .FirstOrDefaultAsync(u => u.Id == request.Id, cancellationToken);

    if (appUnit == null)
    {
      throw new AppUnitNotFoundException($"AppUnit with ID '{request.Id}' not found.", request.Id);
    }

    var appUnitDto = appUnit.ProjectAppUnitToDto();

    return new GetAdminAppUnitByIdResult(appUnitDto);
  }
}