using FastEndpoints;

namespace Accounting.Taxes.Features.AdminTaxes.RemoveTaxFromTaxGroup;

public record RemoveTaxFromTaxGroupRequest(Guid TaxGroupId, Guid TaxId);
public record RemoveTaxFromTaxGroupResponse(Guid TaxId, Guid TaxGroupId);

public class RemoveTaxFromTaxGroupEndpoint : Endpoint<RemoveTaxFromTaxGroupRequest, RemoveTaxFromTaxGroupResponse>
{
  private readonly ISender _sender;

  public RemoveTaxFromTaxGroupEndpoint(ISender sender)
  {
    _sender = sender;
  }

  public override void Configure()
  {
    Delete("/admin/tax-groups/{TaxGroupId}/taxes/{TaxId}");
    Description(x => x
        .WithName("RemoveTaxFromTaxGroup")
        .WithTags("Taxes")
        .Produces<RemoveTaxFromTaxGroupResponse>(StatusCodes.Status200OK)
        .ProducesProblem(StatusCodes.Status404NotFound)
        .ProducesProblem(StatusCodes.Status500InternalServerError));
  }

  public override async Task HandleAsync(RemoveTaxFromTaxGroupRequest req, CancellationToken ct)
  {
    var command = new RemoveTaxFromTaxGroupCommand(req.TaxGroupId, req.TaxId);
    var result = await _sender.Send(command, ct);

    await SendAsync(new RemoveTaxFromTaxGroupResponse(result.TaxId, result.TaxGroupId), cancellation: ct);
  }
}