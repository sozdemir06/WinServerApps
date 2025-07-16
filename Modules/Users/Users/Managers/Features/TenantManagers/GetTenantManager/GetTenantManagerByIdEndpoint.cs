using FastEndpoints;
using Shared.Services.Claims;
using Users.Managers.Dtos;

namespace Users.Managers.Features.TenantManagers.GetTenantManager;

public record GetTenantManagerByIdRequest(Guid Id);
public record GetTenantManagerByIdResponse(ManagerDto Manager);

public class GetTenantManagerByIdEndpoint(ISender sender,IClaimsPrincipalService claimsPrincipalService) : Endpoint<GetTenantManagerByIdRequest, GetTenantManagerByIdResponse>
{
  public override void Configure()
  {
    Get("/tenant/managers/{Id}");
    AllowAnonymous();
    Description(x => x
        .WithName("GetTenantManagerById")
        .WithTags("TenantManagers")
        .Produces<GetTenantManagerByIdResponse>(StatusCodes.Status200OK)
        .ProducesProblem(StatusCodes.Status404NotFound)
        .ProducesProblem(StatusCodes.Status500InternalServerError));
  }

  public override async Task HandleAsync(GetTenantManagerByIdRequest request, CancellationToken ct)
  {
    var tenantId = claimsPrincipalService.GetCurrentTenantId();
    var query = new GetTenantManagerByIdQuery(request.Id, tenantId);
    var result = await sender.Send(query, ct);

    await SendAsync(new GetTenantManagerByIdResponse(result.Manager), StatusCodes.Status200OK, ct);
  }
}