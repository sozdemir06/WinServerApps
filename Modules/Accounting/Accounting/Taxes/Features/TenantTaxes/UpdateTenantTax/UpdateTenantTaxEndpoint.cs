using Accounting.Taxes.Dtos;
using FastEndpoints;

namespace Accounting.Taxes.Features.TenantTaxes.UpdateTenantTax;

public record UpdateTenantTaxRequest(Guid Id, TenantTaxDto TenantTax);
public record UpdateTenantTaxResponse(Guid Id);

public class UpdateTenantTaxEndpoint : Endpoint<UpdateTenantTaxRequest, UpdateTenantTaxResponse>
{
  private readonly ISender _sender;

  public UpdateTenantTaxEndpoint(ISender sender)
  {
    _sender = sender;
  }

  public override void Configure()
  {
    Put("/tenant-taxes/{Id}");
    Description(x => x
        .WithName("UpdateTenantTax")
        .WithTags("Taxes")
        .Produces<UpdateTenantTaxResponse>(StatusCodes.Status200OK)
        .ProducesProblem(StatusCodes.Status400BadRequest)
        .ProducesProblem(StatusCodes.Status404NotFound)
        .ProducesProblem(StatusCodes.Status500InternalServerError));
  }

  public override async Task HandleAsync(UpdateTenantTaxRequest req, CancellationToken ct)
  {
    var command = new UpdateTenantTaxCommand(req.Id, req.TenantTax);
    var result = await _sender.Send(command, ct);

    await SendAsync(new UpdateTenantTaxResponse(result.Id), cancellation: ct);
  }
}