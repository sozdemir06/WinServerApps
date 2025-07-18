using FastEndpoints;

namespace Accounting.TaxGroups.Features.TenantTaxGroups.DeleteTenantTaxGroup;

public record DeleteTenantTaxGroupRequest(Guid Id);
public record DeleteTenantTaxGroupResponse(Guid Id);

public class DeleteTenantTaxGroupEndpoint : Endpoint<DeleteTenantTaxGroupRequest, DeleteTenantTaxGroupResponse>
{
  private readonly ISender _sender;

  public DeleteTenantTaxGroupEndpoint(ISender sender)
  {
    _sender = sender;
  }

  public override void Configure()
  {
    Delete("/tenant-tax-groups/{Id}");
    Description(x => x
        .WithName("DeleteTenantTaxGroup")
        .WithTags("TaxGroups")
        .Produces<DeleteTenantTaxGroupResponse>(StatusCodes.Status200OK)
        .ProducesProblem(StatusCodes.Status404NotFound)
        .ProducesProblem(StatusCodes.Status500InternalServerError));
  }

  public override async Task HandleAsync(DeleteTenantTaxGroupRequest req, CancellationToken ct)
  {
    var command = new DeleteTenantTaxGroupCommand(req.Id);
    var result = await _sender.Send(command, ct);

    await SendAsync(new DeleteTenantTaxGroupResponse(result.Id), cancellation: ct);
  }
}