using FastEndpoints;
using WinApps.Modules.Catalog.Catalog.Categories.Dtos;
using WinApps.Modules.Catalog.Catalog.Categories.QueryParams;

namespace WinApps.Modules.Catalog.Catalog.Categories.Features.TenantCategories.GetTenantCategories;

public record GetTenantCategoriesRequest() : TenantCategoryParams;
public record GetTenantCategoriesResponse(IEnumerable<TenantCategoryDto> Categories, PaginationMetaData MetaData);

public class GetTenantCategoriesEndpoint(ISender sender,IClaimsPrincipalService claimsPrincipalService) : Endpoint<GetTenantCategoriesRequest, GetTenantCategoriesResponse>
{


    public override void Configure()
  {
    Get("/tenant/categories");
    Description(x => x
        .WithName("GetTenantCategories")
        .WithTags("TenantCategories")
        .Produces<GetTenantCategoriesResponse>(StatusCodes.Status200OK)
        .ProducesProblem(StatusCodes.Status500InternalServerError));
  }

  public override async Task HandleAsync(GetTenantCategoriesRequest req, CancellationToken ct)
  {
    var tenantId = claimsPrincipalService.GetCurrentTenantId();
    var query = new GetTenantCategoriesQuery(req, tenantId);
    var result = await sender.Send(query, ct);

    await SendAsync(new GetTenantCategoriesResponse(result.Categories, result.MetaData), cancellation: ct);
  }
}