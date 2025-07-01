using FastEndpoints;


namespace Modules.Users.Users.AppTenants.Features.GetAppTenantById;

public record GetAppTenantByIdRequest(Guid Id);

public record GetAppTenantByIdResponse(AppTenantDto AppTenant);

/// <summary>
/// Endpoint for retrieving a specific application tenant by ID
/// </summary>
public class GetAppTenantByIdEndpoint : Endpoint<GetAppTenantByIdRequest, GetAppTenantByIdResponse>
{
  private readonly ISender _sender;

  public GetAppTenantByIdEndpoint(ISender sender)
  {
    _sender = sender ?? throw new ArgumentNullException(nameof(sender));
  }

  public override void Configure()
  {
    Get("/admin/app-tenants/{id}");
    Description(x => x
        .WithName("GetAppTenantById")
        .WithTags("AppTenants")
        .Produces<GetAppTenantByIdResponse>(StatusCodes.Status200OK)
        .ProducesProblem(StatusCodes.Status404NotFound)
        .ProducesProblem(StatusCodes.Status500InternalServerError));
  }

  public override async Task HandleAsync(GetAppTenantByIdRequest request, CancellationToken ct)
  {
    var query = new GetAppTenantByIdQuery(request.Id);
    var result = await _sender.Send(query, ct);

    await SendAsync(new GetAppTenantByIdResponse(result.AppTenant), cancellation: ct);
  }
}