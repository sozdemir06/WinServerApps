using FastEndpoints;
using Modules.Users.Users.AppTenants.Features.GetAppTenants;


namespace Modules.Users.Users.AppTenants.Features.GetAppTenants;


public record GetAppTenantsRequest() : AppTenantParams;
public record GetAppTenantsResponse(IEnumerable<AppTenantDto> AppTenants, PaginationMetaData MetaData);

public class GetAppTenantsEndpoint(ISender sender) : Endpoint<GetAppTenantsRequest, GetAppTenantsResponse>
{
  public override void Configure()
  {
    Get("admin/app-tenants");
    Description(x => x
        .WithName("GetAppTenants")
        .WithTags("AppTenants")
        .Produces<GetAppTenantsResponse>(StatusCodes.Status200OK)
        .ProducesProblem(StatusCodes.Status500InternalServerError));


  }

  public override async Task HandleAsync(GetAppTenantsRequest req, CancellationToken ct)
  {
    var query = new GetAppTenantsQuery(req);
    var result = await sender.Send(query, ct);

    await SendAsync(new GetAppTenantsResponse(result.AppTenants, result.MetaData), cancellation: ct);
  }
}