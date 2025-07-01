using FastEndpoints;
using Users.Languages.Dtos;
using Users.Languages.Features.CreateLanguage;

namespace Users.Languages.Features.CreateLanguage;

public record CreateLanguageRequest(LanguageDto Language);
public record CreateLanguageResponse(Guid Id);

/// <summary>
/// Endpoint for creating a new language
/// </summary>
public class CreateLanguageEndpoint(ISender sender) : Endpoint<CreateLanguageRequest, CreateLanguageResponse>
{
  public override void Configure()
  {
    Post("/admin/languages");
    Description(x => x
        .WithName("CreateLanguage")
        .WithTags("Languages")
        .Produces<CreateLanguageResponse>(StatusCodes.Status201Created)
        .ProducesProblem(StatusCodes.Status400BadRequest)
        .ProducesProblem(StatusCodes.Status500InternalServerError));
  }

  public override async Task HandleAsync(CreateLanguageRequest request, CancellationToken ct)
  {
    var command = new CreateLanguageCommand(request.Language);
    var result = await sender.Send(command, ct);

    await SendAsync(new CreateLanguageResponse(result.Id), StatusCodes.Status201Created, ct);
  }
}