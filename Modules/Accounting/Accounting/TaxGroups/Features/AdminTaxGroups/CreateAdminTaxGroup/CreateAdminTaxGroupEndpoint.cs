using Accounting.TaxGroups.Dtos;
using FastEndpoints;

namespace Accounting.TaxGroups.Features.AdminTaxGroups.CreateAdminTaxGroup;

public record CreateAdminTaxGroupRequest(TaxGroupDto TaxGroup);
public record CreateAdminTaxGroupResponse(Guid Id);

public class CreateAdminTaxGroupEndpoint : Endpoint<CreateAdminTaxGroupRequest, CreateAdminTaxGroupResponse>
{
  private readonly ISender _sender;

  public CreateAdminTaxGroupEndpoint(ISender sender)
  {
    _sender = sender;
  }

  public override void Configure()
  {
    Post("/admin/tax-groups");
    Description(x => x
        .WithName("CreateAdminTaxGroup")
        .WithTags("TaxGroups")
        .Produces<CreateAdminTaxGroupResponse>(StatusCodes.Status201Created)
        .ProducesProblem(StatusCodes.Status400BadRequest)
        .ProducesProblem(StatusCodes.Status500InternalServerError));
  }

  public override async Task HandleAsync(CreateAdminTaxGroupRequest req, CancellationToken ct)
  {
    var command = new CreateAdminTaxGroupCommand(req.TaxGroup);
    var result = await _sender.Send(command, ct);

    await SendAsync(new CreateAdminTaxGroupResponse(result.Id), StatusCodes.Status201Created, cancellation: ct);
  }
}