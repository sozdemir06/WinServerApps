using FastEndpoints;
using WinApps.Modules.Catalog.Catalog.Categories.Dtos;

namespace WinApps.Modules.Catalog.Catalog.Categories.Features.TenantCategories.UpdateTenantCategory;

public record UpdateTenantCategoryRequest(Guid Id, TenantCategoryDto Category);
public record UpdateTenantCategoryResponse(bool Success);

public class UpdateTenantCategoryEndpoint : Endpoint<UpdateTenantCategoryRequest, UpdateTenantCategoryResponse>
{
  private readonly ISender _sender;

  public UpdateTenantCategoryEndpoint(ISender sender)
  {
    _sender = sender;
  }

  public override void Configure()
  {
    Put("/tenant/categories/{Id}");
    Description(x => x
        .WithName("UpdateTenantCategory")
        .WithTags("TenantCategories")
        .Produces<UpdateTenantCategoryResponse>(StatusCodes.Status200OK)
        .ProducesProblem(StatusCodes.Status400BadRequest)
        .ProducesProblem(StatusCodes.Status404NotFound)
        .ProducesProblem(StatusCodes.Status500InternalServerError));
  }

  public override async Task HandleAsync(UpdateTenantCategoryRequest req, CancellationToken ct)
  {
    var command = new UpdateTenantCategoryCommand(req.Id, req.Category);
    var result = await _sender.Send(command, ct);

    await SendAsync(new UpdateTenantCategoryResponse(result.Success), cancellation: ct);
  }
}