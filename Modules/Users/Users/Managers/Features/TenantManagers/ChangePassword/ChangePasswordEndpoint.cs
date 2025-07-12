using FastEndpoints;
using Users.Managers.Dtos;

namespace Users.Managers.Features.TenantManagers.ChangePassword;

public record ChangePasswordRequest(Guid ManagerId, ChangePasswordDto ChangePassword);
public record ChangePasswordResponse(bool Success);

public class ChangePasswordEndpoint(ISender sender) : Endpoint<ChangePasswordRequest, ChangePasswordResponse>
{
  public override void Configure()
  {
    Put("/tenant/managers/{ManagerId}/change-password");
    AllowAnonymous();
    Description(x => x
        .WithName("ChangeTenantManagerPassword")
        .WithTags("TenantManagers")
        .Produces<ChangePasswordResponse>(StatusCodes.Status200OK)
        .ProducesProblem(StatusCodes.Status400BadRequest)
        .ProducesProblem(StatusCodes.Status404NotFound)
        .ProducesProblem(StatusCodes.Status500InternalServerError));
  }

  public override async Task HandleAsync(ChangePasswordRequest request, CancellationToken ct)
  {
    var command = new ChangePasswordCommand(request.ManagerId, request.ChangePassword);
    var result = await sender.Send(command, ct);

    await SendAsync(new ChangePasswordResponse(result.Success), StatusCodes.Status200OK, ct);
  }
}