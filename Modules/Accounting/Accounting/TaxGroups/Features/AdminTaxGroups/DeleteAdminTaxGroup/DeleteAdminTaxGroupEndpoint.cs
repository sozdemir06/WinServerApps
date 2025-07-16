using FastEndpoints;

namespace Accounting.TaxGroups.Features.AdminTaxGroups.DeleteAdminTaxGroup;

public record DeleteAdminTaxGroupRequest(Guid Id);
public record DeleteAdminTaxGroupResponse(Guid Id);

public class DeleteAdminTaxGroupEndpoint : Endpoint<DeleteAdminTaxGroupRequest, DeleteAdminTaxGroupResponse>
{
  private readonly ISender _sender;

  public DeleteAdminTaxGroupEndpoint(ISender sender)
  {
    _sender = sender;
  }

  public override void Configure()
  {
    Delete("/admin/tax-groups/{Id}");
    Description(x => x
        .WithName("DeleteAdminTaxGroup")
        .WithTags("TaxGroups")
        .Produces<DeleteAdminTaxGroupResponse>(StatusCodes.Status200OK)
        .ProducesProblem(StatusCodes.Status404NotFound)
        .ProducesProblem(StatusCodes.Status500InternalServerError));
  }

  public override async Task HandleAsync(DeleteAdminTaxGroupRequest req, CancellationToken ct)
  {
    var command = new DeleteAdminTaxGroupCommand(req.Id);
    var result = await _sender.Send(command, ct);

    await SendAsync(new DeleteAdminTaxGroupResponse(result.Id), cancellation: ct);
  }
}