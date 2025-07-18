using FastEndpoints;

namespace Accounting.Taxes.Features.TenantTaxes.DeleteTenantTax;

public record DeleteTenantTaxRequest(Guid Id);
public record DeleteTenantTaxResponse(Guid Id);

public class DeleteTenantTaxEndpoint(ISender sender) : Endpoint<DeleteTenantTaxRequest, DeleteTenantTaxResponse>
{

    public override void Configure()
  {
    Delete("/tenant-taxes/{Id}");
    Description(x => x
        .WithName("DeleteTenantTax")
        .WithTags("Taxes")
        .Produces<DeleteTenantTaxResponse>(StatusCodes.Status200OK)
        .ProducesProblem(StatusCodes.Status404NotFound)
        .ProducesProblem(StatusCodes.Status500InternalServerError));
  }

  public override async Task HandleAsync(DeleteTenantTaxRequest req, CancellationToken ct)
  {
    var command = new DeleteTenantTaxCommand(req.Id);
    var result = await sender.Send(command, ct);

    await SendAsync(new DeleteTenantTaxResponse(result.Id), cancellation: ct);
  }
}