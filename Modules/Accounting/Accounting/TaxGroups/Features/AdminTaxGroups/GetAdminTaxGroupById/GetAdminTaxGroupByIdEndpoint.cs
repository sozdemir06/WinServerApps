using Accounting.TaxGroups.Dtos;
using FastEndpoints;

namespace Accounting.TaxGroups.Features.AdminTaxGroups.GetAdminTaxGroupById;

public record GetAdminTaxGroupByIdRequest(Guid Id);
public record GetAdminTaxGroupByIdResponse(TaxGroupDto TaxGroup);

public class GetAdminTaxGroupByIdEndpoint : Endpoint<GetAdminTaxGroupByIdRequest, GetAdminTaxGroupByIdResponse>
{
  private readonly ISender _sender;

  public GetAdminTaxGroupByIdEndpoint(ISender sender)
  {
    _sender = sender;
  }

  public override void Configure()
  {
    Get("/admin/tax-groups/{Id}");
    Description(x => x
        .WithName("GetAdminTaxGroupById")
        .WithTags("TaxGroups")
        .Produces<GetAdminTaxGroupByIdResponse>(StatusCodes.Status200OK)
        .ProducesProblem(StatusCodes.Status404NotFound)
        .ProducesProblem(StatusCodes.Status500InternalServerError));
  }

  public override async Task HandleAsync(GetAdminTaxGroupByIdRequest req, CancellationToken ct)
  {
    var query = new GetAdminTaxGroupByIdQuery(req.Id);
    var result = await _sender.Send(query, ct);

    await SendAsync(new GetAdminTaxGroupByIdResponse(result.TaxGroup), cancellation: ct);
  }
}