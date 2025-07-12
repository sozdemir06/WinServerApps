using FastEndpoints;
using Users.Managers.Dtos;
using WinApps.Modules.Users.Users.Managers.Dtos;

namespace WinApps.Modules.Users.Users.Managers.Features.TenantManagers.TenantLogin;

public record TenantLoginRequest(LoginRequestDto Login);
public record TenantLoginResponse(LoginResponseDto LoginResponse);

public class TenantLoginEndpoint(ISender sender) : Endpoint<TenantLoginRequest, TenantLoginResponse>
{
  public override void Configure()
  {
    Post("/tenant/login");
    AllowAnonymous();
    Description(b => b
        .WithName("TenantLogin")
        .WithTags("Tenant")
        .Produces<TenantLoginResponse>(200, "application/json")
        .ProducesProblem(400)
        .ProducesProblem(500));
  }

  public override async Task HandleAsync(TenantLoginRequest request, CancellationToken ct)
  {
    var command = new TenantLoginCommand(request.Login);
    var result = await sender.Send(command, ct);

    await SendAsync(new TenantLoginResponse(result.LoginResponse), 200, ct);
  }
}