using FastEndpoints;
using WinApps.Modules.Catalog.Catalog.Categories.Features.DeactivateAdminCategory;

namespace WinApps.Modules.Catalog.Catalog.Categories.Features.DeactivateAdminCategory;

public record DeactivateAdminCategoryRequest(Guid Id);
public record DeactivateAdminCategoryResponse(bool Success);

public class DeactivateAdminCategoryEndpoint(ISender sender) : Endpoint<DeactivateAdminCategoryRequest, DeactivateAdminCategoryResponse>
{
  public override void Configure()
  {
    Patch("admin/categories/{Id}/deactivate");
    Description(x => x
        .WithName("DeactivateAdminCategory")
        .WithTags("AdminCategories")
        .Produces<DeactivateAdminCategoryResponse>(StatusCodes.Status200OK)
        .ProducesProblem(StatusCodes.Status404NotFound)
        .ProducesProblem(StatusCodes.Status500InternalServerError));
  }

  public override async Task HandleAsync(DeactivateAdminCategoryRequest req, CancellationToken ct)
  {
    var command = new DeactivateAdminCategoryCommand(req.Id);
    var result = await sender.Send(command, ct);

    await SendAsync(new DeactivateAdminCategoryResponse(result.Success), cancellation: ct);
  }
}