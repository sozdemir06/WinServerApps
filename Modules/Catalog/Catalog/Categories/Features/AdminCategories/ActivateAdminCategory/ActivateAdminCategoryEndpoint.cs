using FastEndpoints;
using WinApps.Modules.Catalog.Catalog.Categories.Features.ActivateAdminCategory;

namespace WinApps.Modules.Catalog.Catalog.Categories.Features.ActivateAdminCategory;

public record ActivateAdminCategoryRequest(Guid Id);
public record ActivateAdminCategoryResponse(bool Success);

public class ActivateAdminCategoryEndpoint(ISender sender) : Endpoint<ActivateAdminCategoryRequest, ActivateAdminCategoryResponse>
{
  public override void Configure()
  {
    Patch("admin/categories/{Id}/activate");
    Description(x => x
        .WithName("ActivateAdminCategory")
        .WithTags("AdminCategories")
        .Produces<ActivateAdminCategoryResponse>(StatusCodes.Status200OK)
        .ProducesProblem(StatusCodes.Status404NotFound)
        .ProducesProblem(StatusCodes.Status500InternalServerError));
  }

  public override async Task HandleAsync(ActivateAdminCategoryRequest req, CancellationToken ct)
  {
    var command = new ActivateAdminCategoryCommand(req.Id);
    var result = await sender.Send(command, ct);

    await SendAsync(new ActivateAdminCategoryResponse(result.Success), cancellation: ct);
  }
}