using FastEndpoints;

namespace Accounting.Taxes.Features.AdminTaxes.DeleteAdminTax;

public record DeleteAdminTaxRequest(Guid Id);
public record DeleteAdminTaxResponse(Guid Id);

public class DeleteAdminTaxEndpoint : Endpoint<DeleteAdminTaxRequest, DeleteAdminTaxResponse>
{
  private readonly ISender _sender;

  public DeleteAdminTaxEndpoint(ISender sender)
  {
    _sender = sender;
  }

  public override void Configure()
  {
    Delete("/admin/taxes/{Id}");
    Description(x => x
        .WithName("DeleteAdminTax")
        .WithTags("Taxes")
        .Produces<DeleteAdminTaxResponse>(StatusCodes.Status200OK)
        .ProducesProblem(StatusCodes.Status404NotFound)
        .ProducesProblem(StatusCodes.Status500InternalServerError));
  }

  public override async Task HandleAsync(DeleteAdminTaxRequest req, CancellationToken ct)
  {
    var command = new DeleteAdminTaxCommand(req.Id);
    var result = await _sender.Send(command, ct);

    await SendAsync(new DeleteAdminTaxResponse(result.Id), cancellation: ct);
  }
}