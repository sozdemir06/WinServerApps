using Accounting.Taxes.Dtos;
using Accounting.Taxes.QueryParams;
using FastEndpoints;

namespace Accounting.Taxes.Features.AdminTaxes.GetAdminTaxes;

public record GetAdminTaxesRequest() : TaxParams;
public record GetAdminTaxesResponse(IEnumerable<TaxDto> Taxes, PaginationMetaData MetaData);

public class GetAdminTaxesEndpoint : Endpoint<GetAdminTaxesRequest, GetAdminTaxesResponse>
{
  private readonly ISender _sender;

  public GetAdminTaxesEndpoint(ISender sender)
  {
    _sender = sender;
  }

  public override void Configure()
  {
    Get("/admin/taxes");
    Description(x => x
        .WithName("GetAdminTaxes")
        .WithTags("Taxes")
        .Produces<GetAdminTaxesResponse>(StatusCodes.Status200OK)
        .ProducesProblem(StatusCodes.Status500InternalServerError));
  }

  public override async Task HandleAsync(GetAdminTaxesRequest req, CancellationToken ct)
  {
    var query = new GetAdminTaxesQuery(req);
    var result = await _sender.Send(query, ct);

    await SendAsync(new GetAdminTaxesResponse(result.Taxes, result.MetaData), cancellation: ct);
  }
}