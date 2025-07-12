using FastEndpoints;
using WinApps.Modules.Catalog.Catalog.Categories.Dtos;

namespace WinApps.Modules.Catalog.Catalog.Categories.Features.CreateAdminCategory;

public record CreateAdminCategoryRequest(AdminCategoryDto Category);
public record CreateAdminCategoryResponse(Guid Id);

public class CreateAdminCategoryEndpoint(ISender sender) : Endpoint<CreateAdminCategoryRequest, CreateAdminCategoryResponse>
{
  public override void Configure()
  {
    Post("admin/categories");
    Description(x => x
        .WithName("CreateAdminCategory")
        .WithTags("AdminCategories")
        .Produces<CreateAdminCategoryResponse>(StatusCodes.Status201Created)
        .ProducesProblem(StatusCodes.Status400BadRequest)
        .ProducesProblem(StatusCodes.Status500InternalServerError));
  }

  public override async Task HandleAsync(CreateAdminCategoryRequest req, CancellationToken ct)
  {
    var command = new CreateAdminCategoryCommand(req.Category);
    var result = await sender.Send(command, ct);

    await SendAsync(new CreateAdminCategoryResponse(result.Id), cancellation: ct);
  }
}