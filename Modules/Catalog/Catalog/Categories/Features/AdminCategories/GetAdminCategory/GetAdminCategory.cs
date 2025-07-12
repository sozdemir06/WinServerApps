using WinApps.Modules.Catalog.Catalog.Categories.DomainExtensions;
using WinApps.Modules.Catalog.Catalog.Categories.Dtos;
using WinApps.Modules.Catalog.Catalog.Categories.Exceptions;

namespace WinApps.Modules.Catalog.Catalog.Categories.Features.GetAdminCategory;

public record GetAdminCategoryQuery(Guid Id) : IQuery<GetAdminCategoryResult>;
public record GetAdminCategoryResult(AdminCategoryDto Category);

public class GetAdminCategoryHandler(CatalogDbContext context) : IQueryHandler<GetAdminCategoryQuery, GetAdminCategoryResult>
{
  public async Task<GetAdminCategoryResult> Handle(GetAdminCategoryQuery request, CancellationToken cancellationToken)
  {
    var category = await context.AdminCategories
        .Include(x => x.Parent)
        .Include(x => x.Children)
        .Include(x => x.Translates)
        .AsNoTracking()
        .FirstOrDefaultAsync(x => x.Id == request.Id, cancellationToken);

    if (category == null)
    {
      throw new AdminCategoryNotFoundException($"Category with ID '{request.Id}' not found.", request.Id);
    }

    var categoryDto = category.ProjectAdminCategoryToDto();

    return new GetAdminCategoryResult(categoryDto);
  }
}