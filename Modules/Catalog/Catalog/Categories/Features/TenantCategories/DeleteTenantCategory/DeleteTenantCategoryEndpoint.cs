using FastEndpoints;

namespace WinApps.Modules.Catalog.Catalog.Categories.Features.TenantCategories.DeleteTenantCategory;

public record DeleteTenantCategoryRequest(Guid Id);
public record DeleteTenantCategoryResponse(bool Success);

public class DeleteTenantCategoryEndpoint(ISender sender,IClaimsPrincipalService claimsPrincipalService) : Endpoint<DeleteTenantCategoryRequest, DeleteTenantCategoryResponse>
{

    public override void Configure()
  {
    Delete("/tenant/categories/{Id}");
    Description(x => x
        .WithName("DeleteTenantCategory")
        .WithTags("TenantCategories")
        .Produces<DeleteTenantCategoryResponse>(StatusCodes.Status200OK)
        .ProducesProblem(StatusCodes.Status400BadRequest)
        .ProducesProblem(StatusCodes.Status404NotFound)
        .ProducesProblem(StatusCodes.Status500InternalServerError));
  }

  public override async Task HandleAsync(DeleteTenantCategoryRequest req, CancellationToken ct)
  {
    var tenantId = claimsPrincipalService.GetCurrentTenantId();
    var command = new DeleteTenantCategoryCommand(req.Id, tenantId);
    var result = await sender.Send(command, ct);

    await SendAsync(new DeleteTenantCategoryResponse(result.Success), cancellation: ct);
  }
}