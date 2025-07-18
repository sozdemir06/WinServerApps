using Accounting.Taxes.Dtos;
using FastEndpoints;


namespace Accounting.Taxes.Features.TenantTaxes.CreateTenantTax;

public record CreateTenantTaxRequest(TenantTaxDto TenantTax);
public record CreateTenantTaxResponse(Guid Id);

public class CreateTenantTaxEndpoint(ISender sender,IClaimsPrincipalService claimsPrincipalService) : Endpoint<CreateTenantTaxRequest, CreateTenantTaxResponse>
{

    public override void Configure()
  {
    Post("/tenant-taxes");
    Description(x => x
        .WithName("CreateTenantTax")
        .WithTags("Taxes")
        .Produces<CreateTenantTaxResponse>(StatusCodes.Status201Created)
        .ProducesProblem(StatusCodes.Status400BadRequest)
        .ProducesProblem(StatusCodes.Status500InternalServerError));
  }

  public override async Task HandleAsync(CreateTenantTaxRequest req, CancellationToken ct)
  {
    var tenantId = claimsPrincipalService.GetCurrentTenantId();
    var command = new CreateTenantTaxCommand(req.TenantTax,tenantId);
    var result = await sender.Send(command, ct);

    await SendAsync(new CreateTenantTaxResponse(result.Id), StatusCodes.Status201Created, cancellation: ct);
  }
}