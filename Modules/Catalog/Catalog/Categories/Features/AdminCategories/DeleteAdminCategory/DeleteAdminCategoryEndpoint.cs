using FastEndpoints;
using WinApps.Modules.Catalog.Catalog.Categories.Features.DeleteAdminCategory;

namespace WinApps.Modules.Catalog.Catalog.Categories.Features.DeleteAdminCategory;

public record DeleteAdminCategoryRequest(Guid Id);
public record DeleteAdminCategoryResponse(bool Success);

public class DeleteAdminCategoryEndpoint(ISender sender) : Endpoint<DeleteAdminCategoryRequest, DeleteAdminCategoryResponse>
{
  public override void Configure()
  {
    Delete("admin/categories/{Id}");
    Description(x => x
        .WithName("DeleteAdminCategory")
        .WithTags("AdminCategories")
        .Produces<DeleteAdminCategoryResponse>(StatusCodes.Status200OK)
        .ProducesProblem(StatusCodes.Status400BadRequest)
        .ProducesProblem(StatusCodes.Status404NotFound)
        .ProducesProblem(StatusCodes.Status500InternalServerError));
  }

  public override async Task HandleAsync(DeleteAdminCategoryRequest req, CancellationToken ct)
  {
    var command = new DeleteAdminCategoryCommand(req.Id);
    var result = await sender.Send(command, ct);

    await SendAsync(new DeleteAdminCategoryResponse(result.Success), cancellation: ct);
  }
}