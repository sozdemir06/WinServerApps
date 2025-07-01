using FastEndpoints;
using Users.Languages.Dtos;
using Users.Languages.Features.UpdateLanguage;

namespace Users.Languages.Features.UpdateLanguage;

public record UpdateLanguageRequest(Guid Id, LanguageDto Language);
public record UpdateLanguageResponse(bool Success);

/// <summary>
/// Endpoint for updating an existing language
/// </summary>
public class UpdateLanguageEndpoint(ISender sender) : Endpoint<UpdateLanguageRequest, UpdateLanguageResponse>
{
  public override void Configure()
  {
    Put("/admin/languages/{id}");
    Description(x => x
        .WithName("UpdateLanguage")
        .WithTags("Languages")
        .Produces<UpdateLanguageResponse>(StatusCodes.Status200OK)
        .ProducesProblem(StatusCodes.Status400BadRequest)
        .ProducesProblem(StatusCodes.Status404NotFound)
        .ProducesProblem(StatusCodes.Status500InternalServerError));
  }

  public override async Task HandleAsync(UpdateLanguageRequest request, CancellationToken ct)
  {
    var command = new UpdateLanguageCommand(request.Id, request.Language);
    var result = await sender.Send(command, ct);

    await SendAsync(new UpdateLanguageResponse(result.Success), cancellation: ct);
  }
}