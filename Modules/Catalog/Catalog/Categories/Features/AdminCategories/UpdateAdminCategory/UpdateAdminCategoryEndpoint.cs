using FastEndpoints;
using WinApps.Modules.Catalog.Catalog.Categories.Dtos;

namespace WinApps.Modules.Catalog.Catalog.Categories.Features.UpdateAdminCategory;

public record UpdateAdminCategoryRequest(Guid Id, AdminCategoryDto Category);
public record UpdateAdminCategoryResponse(bool Success);

public class UpdateAdminCategoryEndpoint(ISender sender) : Endpoint<UpdateAdminCategoryRequest, UpdateAdminCategoryResponse>
{
  public override void Configure()
  {
    Put("admin/categories/{Id}");
    Description(x => x
        .WithName("UpdateAdminCategory")
        .WithTags("AdminCategories")
        .Produces<UpdateAdminCategoryResponse>(StatusCodes.Status200OK)
        .ProducesProblem(StatusCodes.Status400BadRequest)
        .ProducesProblem(StatusCodes.Status404NotFound)
        .ProducesProblem(StatusCodes.Status500InternalServerError));
  }

  public override async Task HandleAsync(UpdateAdminCategoryRequest req, CancellationToken ct)
  {
    var command = new UpdateAdminCategoryCommand(req.Id, req.Category);
    var result = await sender.Send(command, ct);

    await SendAsync(new UpdateAdminCategoryResponse(result.Success), cancellation: ct);
  }
}