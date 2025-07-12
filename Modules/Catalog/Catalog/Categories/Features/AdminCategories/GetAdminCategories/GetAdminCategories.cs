using WinApps.Modules.Catalog.Catalog.Categories.DomainExtensions;
using WinApps.Modules.Catalog.Catalog.Categories.Dtos;
using WinApps.Modules.Catalog.Catalog.Categories.QueryParams;

namespace WinApps.Modules.Catalog.Catalog.Categories.Features.GetAdminCategories;

public record GetAdminCategoriesQuery(AdminCategoryParams Parameters) : IQuery<GetAdminCategoriesResult>,ICachableRequest
{
  public string CacheKey => CacheKeyGenerator.GenerateKey(CacheKeys.AdminCategories, Parameters);
  public string CacheGroupKey => CacheKeys.AdminCategories;
  public TimeSpan? CacheExpiration => null;
}

public record GetAdminCategoriesResult(IEnumerable<AdminCategoryDto> Categories, PaginationMetaData MetaData);

public class GetAdminCategoriesHandler(CatalogDbContext context) : IQueryHandler<GetAdminCategoriesQuery, GetAdminCategoriesResult>
{
  public async Task<GetAdminCategoriesResult> Handle(GetAdminCategoriesQuery request, CancellationToken cancellationToken)
  {
    var query = context.AdminCategories
                .Include(x => x.Translates)
                  .ThenInclude(x => x!.Language)
                .Include(x => x.Parent)
                .ThenInclude(x => x!.Translates)
                  .ThenInclude(x => x!.Language)
                .AsNoTracking()
                .AsQueryable()
                .ProjectAdminCategoryListToDto();

    var categories = await PagedList<AdminCategoryDto>.ToPagedList(
        query,
        request.Parameters.PageNumber,
        request.Parameters.PageSize,
        cancellationToken);

    var categoriesDto = categories.Adapt<IEnumerable<AdminCategoryDto>>();

    return new GetAdminCategoriesResult(categoriesDto, categories.MetaData);
  }
}