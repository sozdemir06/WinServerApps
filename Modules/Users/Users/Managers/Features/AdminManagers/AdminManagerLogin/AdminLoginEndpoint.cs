using FastEndpoints;
using Users.Managers.Dtos;
using WinApps.Modules.Users.Users.Managers.Dtos;

namespace WinApps.Modules.Users.Users.Managers.Features.AdminManagers.AdminManagerLogin;

public record AdminLoginRequest(LoginRequestDto Login);
public record AdminLoginResponse(LoginResponseDto LoginResponse);

public class AdminLoginEndpoint(ISender sender) : Endpoint<AdminLoginRequest, AdminLoginResponse>
{
  public override void Configure()
  {
    Post("/admin/login");
    AllowAnonymous();
    Description(b => b
        .WithName("AdminLogin")
        .WithTags("Admin")
        .Produces<AdminLoginResponse>(200, "application/json")
        .ProducesProblem(400)
        .ProducesProblem(500));
  }

  public override async Task HandleAsync(AdminLoginRequest request, CancellationToken ct)
  {
    var command = new AdminLoginCommand(request.Login);
    var result = await sender.Send(command, ct);

    await SendAsync(new AdminLoginResponse(result.LoginResponse), 200, ct);
  }
}