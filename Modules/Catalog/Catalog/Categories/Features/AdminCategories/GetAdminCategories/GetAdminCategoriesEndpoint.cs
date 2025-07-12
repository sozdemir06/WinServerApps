using FastEndpoints;

using WinApps.Modules.Catalog.Catalog.Categories.Dtos;
using WinApps.Modules.Catalog.Catalog.Categories.QueryParams;

namespace WinApps.Modules.Catalog.Catalog.Categories.Features.GetAdminCategories;

public record GetAdminCategoriesRequest() : AdminCategoryParams;
public record GetAdminCategoriesResponse(IEnumerable<AdminCategoryDto> Categories, PaginationMetaData MetaData);

public class GetAdminCategoriesEndpoint(ISender sender) : Endpoint<GetAdminCategoriesRequest, GetAdminCategoriesResponse>
{
  public override void Configure()
  {
    Get("admin/categories");
    Description(x => x
        .WithName("GetAdminCategories")
        .WithTags("AdminCategories")
        .Produces<GetAdminCategoriesResponse>(StatusCodes.Status200OK)
        .ProducesProblem(StatusCodes.Status500InternalServerError));
  }

  public override async Task HandleAsync(GetAdminCategoriesRequest req, CancellationToken ct)
  {
    var query = new GetAdminCategoriesQuery(req);
    var result = await sender.Send(query, ct);

    await SendAsync(new GetAdminCategoriesResponse(result.Categories, result.MetaData), cancellation: ct);
  }
}