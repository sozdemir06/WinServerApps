using FastEndpoints;
using Shared.Services.Claims;
using Users.Managers.Dtos;

namespace Users.Managers.Features.TenantManagers.CreateTenantManager;

public record CreateTenantManagerRequest(ManagerDto Manager);
public record CreateTenantManagerResponse(Guid Id);

public class CreateTenantManagerEndpoint(ISender sender, IClaimsPrincipalService claimsPrincipalService) : Endpoint<CreateTenantManagerRequest, CreateTenantManagerResponse>
{
  public override void Configure()
  {
    Post("/tenant/managers");
    AllowAnonymous();
    Description(x => x
        .WithName("CreateTenantManager")
        .WithTags("TenantManagers")
        .Produces<CreateTenantManagerResponse>(StatusCodes.Status201Created)
        .ProducesProblem(StatusCodes.Status400BadRequest)
        .ProducesProblem(StatusCodes.Status500InternalServerError));
  }

  public override async Task HandleAsync(CreateTenantManagerRequest request, CancellationToken ct)
  {
    var tenantId = claimsPrincipalService.GetCurrentTenantId();
    var command = new CreateTenantManagerCommand(request.Manager, tenantId);
    var result = await sender.Send(command, ct);

    await SendAsync(new CreateTenantManagerResponse(result.Id), StatusCodes.Status201Created, ct);
  }
}