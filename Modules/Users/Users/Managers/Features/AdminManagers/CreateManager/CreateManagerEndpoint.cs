using FastEndpoints;
using Users.Managers.Dtos;

namespace Users.Managers.Features.AdminManagers.CreateManager;

public record CreateManagerRequest(ManagerDto Manager);
public record CreateManagerResponse(Guid Id);

public class CreateManagerEndpoint(ISender sender) : Endpoint<CreateManagerRequest, CreateManagerResponse>
{
  public override void Configure()
  {
    Post("/admin/managers");
    AllowAnonymous();
    Description(x => x
        .WithName("CreateManager")
        .WithTags("AdminManagers")
        .Produces<CreateManagerResponse>(StatusCodes.Status201Created)
        .ProducesProblem(StatusCodes.Status400BadRequest)
        .ProducesProblem(StatusCodes.Status500InternalServerError));
  }

  public override async Task HandleAsync(CreateManagerRequest request, CancellationToken ct)
  {
    var command = new CreateManagerCommand(request.Manager);
    var result = await sender.Send(command, ct);

    await SendAsync(new CreateManagerResponse(result.Id), StatusCodes.Status201Created, ct);
  }
}