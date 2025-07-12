using FastEndpoints;
using WinApps.Modules.Catalog.Catalog.Categories.Dtos;

namespace WinApps.Modules.Catalog.Catalog.Categories.Features.GetAdminCategory;

public record GetAdminCategoryRequest(Guid Id);
public record GetAdminCategoryResponse(AdminCategoryDto Category);

public class GetAdminCategoryEndpoint(ISender sender) : Endpoint<GetAdminCategoryRequest, GetAdminCategoryResponse>
{
  public override void Configure()
  {
    Get("admin/categories/{Id}");
    Description(x => x
        .WithName("GetAdminCategory")
        .WithTags("AdminCategories")
        .Produces<GetAdminCategoryResponse>(StatusCodes.Status200OK)
        .ProducesProblem(StatusCodes.Status404NotFound)
        .ProducesProblem(StatusCodes.Status500InternalServerError));
  }

  public override async Task HandleAsync(GetAdminCategoryRequest req, CancellationToken ct)
  {
    var query = new GetAdminCategoryQuery(req.Id);
    var result = await sender.Send(query, ct);

    await SendAsync(new GetAdminCategoryResponse(result.Category), cancellation: ct);
  }
}