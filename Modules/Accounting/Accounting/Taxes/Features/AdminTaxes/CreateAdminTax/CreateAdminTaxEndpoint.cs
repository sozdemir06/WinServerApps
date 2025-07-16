using Accounting.Taxes.Dtos;
using FastEndpoints;

namespace Accounting.Taxes.Features.AdminTaxes.CreateAdminTax;

public record CreateAdminTaxRequest(TaxDto Tax);
public record CreateAdminTaxResponse(Guid Id);

public class CreateAdminTaxEndpoint : Endpoint<CreateAdminTaxRequest, CreateAdminTaxResponse>
{
  private readonly ISender _sender;

  public CreateAdminTaxEndpoint(ISender sender)
  {
    _sender = sender;
  }

  public override void Configure()
  {
    Post("/admin/taxes");
    Description(x => x
        .WithName("CreateAdminTax")
        .WithTags("Taxes")
        .Produces<CreateAdminTaxResponse>(StatusCodes.Status201Created)
        .ProducesProblem(StatusCodes.Status400BadRequest)
        .ProducesProblem(StatusCodes.Status500InternalServerError));
  }

  public override async Task HandleAsync(CreateAdminTaxRequest req, CancellationToken ct)
  {
    var command = new CreateAdminTaxCommand(req.Tax);
    var result = await _sender.Send(command, ct);

    await SendAsync(new CreateAdminTaxResponse(result.Id), StatusCodes.Status201Created, cancellation: ct);
  }
}