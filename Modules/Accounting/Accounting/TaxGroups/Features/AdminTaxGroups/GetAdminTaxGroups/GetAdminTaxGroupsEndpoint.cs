using Accounting.TaxGroups.Dtos;
using Accounting.TaxGroups.QueryParams;
using FastEndpoints;

namespace Accounting.TaxGroups.Features.AdminTaxGroups.GetAdminTaxGroups;

public record GetAdminTaxGroupsRequest() : TaxGroupParams;
public record GetAdminTaxGroupsResponse(IEnumerable<TaxGroupDto> TaxGroups, PaginationMetaData MetaData);

public class GetAdminTaxGroupsEndpoint : Endpoint<GetAdminTaxGroupsRequest, GetAdminTaxGroupsResponse>
{
  private readonly ISender _sender;

  public GetAdminTaxGroupsEndpoint(ISender sender)
  {
    _sender = sender;
  }

  public override void Configure()
  {
    Get("/admin/tax-groups");
    Description(x => x
        .WithName("GetAdminTaxGroups")
        .WithTags("TaxGroups")
        .Produces<GetAdminTaxGroupsResponse>(StatusCodes.Status200OK)
        .ProducesProblem(StatusCodes.Status500InternalServerError));
  }

  public override async Task HandleAsync(GetAdminTaxGroupsRequest req, CancellationToken ct)
  {
    var query = new GetAdminTaxGroupsQuery(req);
    var result = await _sender.Send(query, ct);

    await SendAsync(new GetAdminTaxGroupsResponse(result.TaxGroups, result.MetaData), cancellation: ct);
  }
}