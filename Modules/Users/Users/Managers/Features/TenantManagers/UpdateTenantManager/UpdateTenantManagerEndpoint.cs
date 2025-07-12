using FastEndpoints;
using Shared.Services.Claims;
using Users.Managers.Dtos;

namespace Users.Managers.Features.TenantManagers.UpdateTenantManager;

public record UpdateTenantManagerRequest(Guid Id, ManagerDto Manager);
public record UpdateTenantManagerResponse(bool Success);

public class UpdateTenantManagerEndpoint(ISender sender, IClaimsPrincipalService claimsPrincipalService) : Endpoint<UpdateTenantManagerRequest, UpdateTenantManagerResponse>
{
  public override void Configure()
  {
    Put("/tenant/managers/{Id}");
    AllowAnonymous();
    Description(x => x
        .WithName("UpdateTenantManager")
        .WithTags("TenantManagers")
        .Produces<UpdateTenantManagerResponse>(StatusCodes.Status200OK)
        .ProducesProblem(StatusCodes.Status400BadRequest)
        .ProducesProblem(StatusCodes.Status404NotFound)
        .ProducesProblem(StatusCodes.Status500InternalServerError));
  }

  public override async Task HandleAsync(UpdateTenantManagerRequest request, CancellationToken ct)
  {
    var tenantId = claimsPrincipalService.GetCurrentTenantId();
    var command = new UpdateTenantManagerCommand(request.Id, request.Manager, tenantId);
    var result = await sender.Send(command, ct);

    await SendAsync(new UpdateTenantManagerResponse(result.Success), StatusCodes.Status200OK, ct);
  }
}