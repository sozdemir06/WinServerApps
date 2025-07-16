using FastEndpoints;

namespace WinApps.Modules.Catalog.Catalog.Categories.Features.TenantCategories.ActivateTenantCategory;

public record ActivateTenantCategoryRequest(Guid Id);
public record ActivateTenantCategoryResponse(bool Success);

public class ActivateTenantCategoryEndpoint : Endpoint<ActivateTenantCategoryRequest, ActivateTenantCategoryResponse>
{
  private readonly ISender _sender;

  public ActivateTenantCategoryEndpoint(ISender sender)
  {
    _sender = sender;
  }

  public override void Configure()
  {
    Patch("/tenant/categories/{Id}/activate");
    Description(x => x
        .WithName("ActivateTenantCategory")
        .WithTags("TenantCategories")
        .Produces<ActivateTenantCategoryResponse>(StatusCodes.Status200OK)
        .ProducesProblem(StatusCodes.Status404NotFound)
        .ProducesProblem(StatusCodes.Status500InternalServerError));
  }

  public override async Task HandleAsync(ActivateTenantCategoryRequest req, CancellationToken ct)
  {
    var command = new ActivateTenantCategoryCommand(req.Id);
    var result = await _sender.Send(command, ct);

    await SendAsync(new ActivateTenantCategoryResponse(result.Success), cancellation: ct);
  }
}