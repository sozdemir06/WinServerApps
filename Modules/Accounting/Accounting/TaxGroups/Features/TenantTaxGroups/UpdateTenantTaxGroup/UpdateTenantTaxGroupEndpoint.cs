using Accounting.TaxGroups.Dtos;
using FastEndpoints;

namespace Accounting.TaxGroups.Features.TenantTaxGroups.UpdateTenantTaxGroup;

public record UpdateTenantTaxGroupRequest(Guid Id, TenantTaxGroupDto TenantTaxGroup);
public record UpdateTenantTaxGroupResponse(Guid Id);

public class UpdateTenantTaxGroupEndpoint : Endpoint<UpdateTenantTaxGroupRequest, UpdateTenantTaxGroupResponse>
{
  private readonly ISender _sender;

  public UpdateTenantTaxGroupEndpoint(ISender sender)
  {
    _sender = sender;
  }

  public override void Configure()
  {
    Put("/tenant-tax-groups/{Id}");
    Description(x => x
        .WithName("UpdateTenantTaxGroup")
        .WithTags("TaxGroups")
        .Produces<UpdateTenantTaxGroupResponse>(StatusCodes.Status200OK)
        .ProducesProblem(StatusCodes.Status400BadRequest)
        .ProducesProblem(StatusCodes.Status404NotFound)
        .ProducesProblem(StatusCodes.Status500InternalServerError));
  }

  public override async Task HandleAsync(UpdateTenantTaxGroupRequest req, CancellationToken ct)
  {
    var command = new UpdateTenantTaxGroupCommand(req.Id, req.TenantTaxGroup);
    var result = await _sender.Send(command, ct);

    await SendAsync(new UpdateTenantTaxGroupResponse(result.Id), cancellation: ct);
  }
}