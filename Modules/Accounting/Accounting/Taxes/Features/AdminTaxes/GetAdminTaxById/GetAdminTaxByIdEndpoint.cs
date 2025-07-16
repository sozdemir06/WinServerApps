using Accounting.Taxes.Dtos;
using FastEndpoints;

namespace Accounting.Taxes.Features.AdminTaxes.GetAdminTaxById;

public record GetAdminTaxByIdRequest(Guid Id);
public record GetAdminTaxByIdResponse(TaxDto Tax);

public class GetAdminTaxByIdEndpoint : Endpoint<GetAdminTaxByIdRequest, GetAdminTaxByIdResponse>
{
  private readonly ISender _sender;

  public GetAdminTaxByIdEndpoint(ISender sender)
  {
    _sender = sender;
  }

  public override void Configure()
  {
    Get("/admin/taxes/{Id}");
    Description(x => x
        .WithName("GetAdminTaxById")
        .WithTags("Taxes")
        .Produces<GetAdminTaxByIdResponse>(StatusCodes.Status200OK)
        .ProducesProblem(StatusCodes.Status404NotFound)
        .ProducesProblem(StatusCodes.Status500InternalServerError));
  }

  public override async Task HandleAsync(GetAdminTaxByIdRequest req, CancellationToken ct)
  {
    var query = new GetAdminTaxByIdQuery(req.Id);
    var result = await _sender.Send(query, ct);

    await SendAsync(new GetAdminTaxByIdResponse(result.Tax), cancellation: ct);
  }
}