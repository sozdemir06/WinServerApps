using Accounting.TaxGroups.Dtos;
using FastEndpoints;

namespace Accounting.TaxGroups.Features.AdminTaxGroups.UpdateAdminTaxGroup;

public record UpdateAdminTaxGroupRequest(Guid Id, TaxGroupDto TaxGroup);
public record UpdateAdminTaxGroupResponse(Guid Id);

public class UpdateAdminTaxGroupEndpoint : Endpoint<UpdateAdminTaxGroupRequest, UpdateAdminTaxGroupResponse>
{
  private readonly ISender _sender;

  public UpdateAdminTaxGroupEndpoint(ISender sender)
  {
    _sender = sender;
  }

  public override void Configure()
  {
    Put("/admin/tax-groups/{Id}");
    Description(x => x
        .WithName("UpdateAdminTaxGroup")
        .WithTags("TaxGroups")
        .Produces<UpdateAdminTaxGroupResponse>(StatusCodes.Status200OK)
        .ProducesProblem(StatusCodes.Status400BadRequest)
        .ProducesProblem(StatusCodes.Status404NotFound)
        .ProducesProblem(StatusCodes.Status500InternalServerError));
  }

  public override async Task HandleAsync(UpdateAdminTaxGroupRequest req, CancellationToken ct)
  {
    var command = new UpdateAdminTaxGroupCommand(req.Id, req.TaxGroup);
    var result = await _sender.Send(command, ct);

    await SendAsync(new UpdateAdminTaxGroupResponse(result.Id), cancellation: ct);
  }
}