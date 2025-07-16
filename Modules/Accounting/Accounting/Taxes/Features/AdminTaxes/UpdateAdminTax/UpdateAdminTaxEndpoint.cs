using Accounting.Taxes.Dtos;
using FastEndpoints;

namespace Accounting.Taxes.Features.AdminTaxes.UpdateAdminTax;

public record UpdateAdminTaxRequest(Guid Id, TaxDto Tax);
public record UpdateAdminTaxResponse(Guid Id);

public class UpdateAdminTaxEndpoint : Endpoint<UpdateAdminTaxRequest, UpdateAdminTaxResponse>
{
  private readonly ISender _sender;

  public UpdateAdminTaxEndpoint(ISender sender)
  {
    _sender = sender;
  }

  public override void Configure()
  {
    Put("/admin/taxes/{Id}");
    Description(x => x
        .WithName("UpdateAdminTax")
        .WithTags("Taxes")
        .Produces<UpdateAdminTaxResponse>(StatusCodes.Status200OK)
        .ProducesProblem(StatusCodes.Status400BadRequest)
        .ProducesProblem(StatusCodes.Status404NotFound)
        .ProducesProblem(StatusCodes.Status500InternalServerError));
  }

  public override async Task HandleAsync(UpdateAdminTaxRequest req, CancellationToken ct)
  {
    var command = new UpdateAdminTaxCommand(req.Id, req.Tax);
    var result = await _sender.Send(command, ct);

    await SendAsync(new UpdateAdminTaxResponse(result.Id), cancellation: ct);
  }
}