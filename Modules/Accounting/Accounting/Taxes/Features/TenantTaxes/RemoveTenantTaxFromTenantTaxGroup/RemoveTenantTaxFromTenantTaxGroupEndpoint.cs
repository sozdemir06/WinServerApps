using FastEndpoints;

namespace Accounting.Taxes.Features.TenantTaxes.RemoveTenantTaxFromTenantTaxGroup;

public record RemoveTenantTaxFromTenantTaxGroupRequest(Guid TenantTaxGroupId, Guid TenantTaxId);
public record RemoveTenantTaxFromTenantTaxGroupResponse(Guid TenantTaxId, Guid TenantTaxGroupId);

public class RemoveTenantTaxFromTenantTaxGroupEndpoint : Endpoint<RemoveTenantTaxFromTenantTaxGroupRequest, RemoveTenantTaxFromTenantTaxGroupResponse>
{
  private readonly ISender _sender;

  public RemoveTenantTaxFromTenantTaxGroupEndpoint(ISender sender)
  {
    _sender = sender;
  }

  public override void Configure()
  {
    Delete("/tenant-tax-groups/{TenantTaxGroupId}/taxes/{TenantTaxId}");
    Description(x => x
        .WithName("RemoveTenantTaxFromTenantTaxGroup")
        .WithTags("Taxes")
        .Produces<RemoveTenantTaxFromTenantTaxGroupResponse>(StatusCodes.Status200OK)
        .ProducesProblem(StatusCodes.Status404NotFound)
        .ProducesProblem(StatusCodes.Status500InternalServerError));
  }

  public override async Task HandleAsync(RemoveTenantTaxFromTenantTaxGroupRequest req, CancellationToken ct)
  {
    var command = new RemoveTenantTaxFromTenantTaxGroupCommand(req.TenantTaxGroupId, req.TenantTaxId);
    var result = await _sender.Send(command, ct);

    await SendAsync(new RemoveTenantTaxFromTenantTaxGroupResponse(result.TenantTaxId, result.TenantTaxGroupId), cancellation: ct);
  }
}