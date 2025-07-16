using FastEndpoints;

namespace WinApps.Modules.Catalog.Catalog.Categories.Features.TenantCategories.DeactivateTenantCategory;

public record DeactivateTenantCategoryRequest(Guid Id);
public record DeactivateTenantCategoryResponse(bool Success);

public class DeactivateTenantCategoryEndpoint : Endpoint<DeactivateTenantCategoryRequest, DeactivateTenantCategoryResponse>
{
  private readonly ISender _sender;

  public DeactivateTenantCategoryEndpoint(ISender sender)
  {
    _sender = sender;
  }

  public override void Configure()
  {
    Patch("/tenant/categories/{Id}/deactivate");
    Description(x => x
        .WithName("DeactivateTenantCategory")
        .WithTags("TenantCategories")
        .Produces<DeactivateTenantCategoryResponse>(StatusCodes.Status200OK)
        .ProducesProblem(StatusCodes.Status404NotFound)
        .ProducesProblem(StatusCodes.Status500InternalServerError));
  }

  public override async Task HandleAsync(DeactivateTenantCategoryRequest req, CancellationToken ct)
  {
    var command = new DeactivateTenantCategoryCommand(req.Id);
    var result = await _sender.Send(command, ct);

    await SendAsync(new DeactivateTenantCategoryResponse(result.Success), cancellation: ct);
  }
}