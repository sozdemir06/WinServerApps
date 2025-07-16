using Accounting.Taxes.Dtos;
using FastEndpoints;

namespace Accounting.Taxes.Features.AdminTaxes.AddTaxToTaxGroup;

public record AddTaxToTaxGroupRequest(Guid TaxGroupId, TaxDto Tax);
public record AddTaxToTaxGroupResponse(Guid TaxId, Guid TaxGroupId);

public class AddTaxToTaxGroupEndpoint : Endpoint<AddTaxToTaxGroupRequest, AddTaxToTaxGroupResponse>
{
  private readonly ISender _sender;

  public AddTaxToTaxGroupEndpoint(ISender sender)
  {
    _sender = sender;
  }

  public override void Configure()
  {
    Post("/admin/tax-groups/{TaxGroupId}/taxes");
    Description(x => x
        .WithName("AddTaxToTaxGroup")
        .WithTags("Taxes")
        .Produces<AddTaxToTaxGroupResponse>(StatusCodes.Status201Created)
        .ProducesProblem(StatusCodes.Status400BadRequest)
        .ProducesProblem(StatusCodes.Status404NotFound)
        .ProducesProblem(StatusCodes.Status500InternalServerError));
  }

  public override async Task HandleAsync(AddTaxToTaxGroupRequest req, CancellationToken ct)
  {
    var command = new AddTaxToTaxGroupCommand(req.TaxGroupId, req.Tax);
    var result = await _sender.Send(command, ct);

    await SendAsync(new AddTaxToTaxGroupResponse(result.TaxId, result.TaxGroupId), StatusCodes.Status201Created, cancellation: ct);
  }
}