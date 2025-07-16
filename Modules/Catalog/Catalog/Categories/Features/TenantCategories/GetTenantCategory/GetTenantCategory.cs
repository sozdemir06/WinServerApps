using WinApps.Modules.Catalog.Catalog.Categories.DomainExtensions;
using WinApps.Modules.Catalog.Catalog.Categories.Dtos;
using WinApps.Modules.Catalog.Catalog.Categories.Exceptions;

namespace WinApps.Modules.Catalog.Catalog.Categories.Features.TenantCategories.GetTenantCategory;

public record GetTenantCategoryQuery(Guid Id, Guid? TenantId = null) : IQuery<GetTenantCategoryResult>, ICachableRequest
{
  public string CacheKey => CacheKeyGenerator.GenerateKey(CacheKeys.TenantCategories, Id, TenantId!);
  public string CacheGroupKey => CacheKeys.TenantCategories;
  public TimeSpan? CacheExpiration => null;
}

public record GetTenantCategoryResult(TenantCategoryDto Category);

public class GetTenantCategoryHandler(CatalogDbContext context) : IQueryHandler<GetTenantCategoryQuery, GetTenantCategoryResult>
{
  public async Task<GetTenantCategoryResult> Handle(GetTenantCategoryQuery request, CancellationToken cancellationToken)
  {
    var category = await context.TenantCategories
        .Include(x => x.Translates)
          .ThenInclude(x => x!.Language)
        .Include(x => x.Parent)
          .ThenInclude(x => x!.Translates)
            .ThenInclude(x => x!.Language)
        .AsNoTracking()
        .FirstOrDefaultAsync(x => x.Id == request.Id, cancellationToken);

    if (category == null)
    {
      throw new TenantCategoryNotFoundException($"Category with ID '{request.Id}' not found.", request.Id);
    }

    var categoryDto = category.ProjectTenantCategoryToDto();

    return new GetTenantCategoryResult(categoryDto);
  }
}