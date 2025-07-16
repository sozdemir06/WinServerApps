using FastEndpoints;
using Shared.Services.Claims;
using WinApps.Modules.Catalog.Catalog.Categories.Dtos;

namespace WinApps.Modules.Catalog.Catalog.Categories.Features.TenantCategories.GetTenantCategory;

public record GetTenantCategoryRequest(Guid Id);
public record GetTenantCategoryResponse(TenantCategoryDto Category);

public class GetTenantCategoryEndpoint(ISender sender,IClaimsPrincipalService claimsPrincipalService) : Endpoint<GetTenantCategoryRequest, GetTenantCategoryResponse>
{
  public override void Configure()
  {
    Get("/tenant/categories/{Id}");
    Description(x => x
        .WithName("GetTenantCategory")
        .WithTags("TenantCategories")
        .Produces<GetTenantCategoryResponse>(StatusCodes.Status200OK)
        .ProducesProblem(StatusCodes.Status404NotFound)
        .ProducesProblem(StatusCodes.Status500InternalServerError));
  }

  public override async Task HandleAsync(GetTenantCategoryRequest req, CancellationToken ct)
  {
    var tenantId = claimsPrincipalService.GetCurrentTenantId();
    var query = new GetTenantCategoryQuery(req.Id, tenantId);
    var result = await sender.Send(query, ct);

    await SendAsync(new GetTenantCategoryResponse(result.Category), cancellation: ct);
  }
}