using WinApps.Modules.Catalog.Catalog.Categories.Dtos;
using FastEndpoints;

namespace WinApps.Modules.Catalog.Catalog.Categories.Features.TenantCategories.CreateTenantCategory;

public record CreateTenantCategoryRequest(TenantCategoryDto Category);
public record CreateTenantCategoryResponse(Guid Id);

public class CreateTenantCategoryEndpoint(ISender sender,IClaimsPrincipalService claimsPrincipalService) : Endpoint<CreateTenantCategoryRequest, CreateTenantCategoryResponse>
{

    public override void Configure()
  {
    Post("/tenant/categories");
    Description(x => x
        .WithName("CreateTenantCategory")
        .WithTags("TenantCategories")
        .Produces<CreateTenantCategoryResponse>(StatusCodes.Status201Created)
        .ProducesProblem(StatusCodes.Status400BadRequest)
        .ProducesProblem(StatusCodes.Status500InternalServerError)); 
  }

  public override async Task HandleAsync(CreateTenantCategoryRequest req, CancellationToken ct)
  {
    var tenantId = claimsPrincipalService.GetCurrentTenantId();
    var command = new CreateTenantCategoryCommand(req.Category, tenantId);
    var result = await sender.Send(command, ct);

    await SendAsync(new CreateTenantCategoryResponse(result.Id), StatusCodes.Status201Created, cancellation: ct);
  }
} 