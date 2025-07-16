using WinApps.Modules.Catalog.Catalog.Categories.DomainExtensions;
using WinApps.Modules.Catalog.Catalog.Categories.Dtos;
using WinApps.Modules.Catalog.Catalog.Categories.QueryParams;

namespace WinApps.Modules.Catalog.Catalog.Categories.Features.TenantCategories.GetTenantCategories;

public record GetTenantCategoriesQuery(TenantCategoryParams Parameters, Guid? TenantId = null) : IQuery<GetTenantCategoriesResult>, ICachableRequest
{
  public string CacheKey => CacheKeyGenerator.GenerateKey(CacheKeys.TenantCategories, Parameters, TenantId!);
  public string CacheGroupKey => CacheKeys.TenantCategories;
  public TimeSpan? CacheExpiration => null;
}

public record GetTenantCategoriesResult(IEnumerable<TenantCategoryDto> Categories, PaginationMetaData MetaData);

public class GetTenantCategoriesHandler(CatalogDbContext context) : IQueryHandler<GetTenantCategoriesQuery, GetTenantCategoriesResult>
{
  public async Task<GetTenantCategoriesResult> Handle(GetTenantCategoriesQuery request, CancellationToken cancellationToken)
  {
    var query = context.TenantCategories
                .Include(x => x.Translates)
                  .ThenInclude(x => x!.Language)
                .Include(x => x.Parent)
                .ThenInclude(x => x!.Translates)
                  .ThenInclude(x => x!.Language)
                .AsNoTracking()
                .ApplyTenantCategoryFilters(request.Parameters)
                .ApplyTenantCategoryOrdering()
                .ProjectTenantCategoryListToDto();

    var categories = await PagedList<TenantCategoryDto>.ToPagedList(
        query,
        request.Parameters.PageNumber,
        request.Parameters.PageSize,
        cancellationToken);

    return new GetTenantCategoriesResult(categories, categories.MetaData);
  }
}