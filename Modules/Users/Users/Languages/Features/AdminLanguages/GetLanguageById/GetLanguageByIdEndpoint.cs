using FastEndpoints;
using Users.Languages.Dtos;
using Users.Languages.Features.GetLanguageById;

namespace Users.Languages.Features.GetLanguageById;

public record GetLanguageByIdRequest(Guid Id);
public record GetLanguageByIdResponse(LanguageDto Language);

/// <summary>
/// Endpoint for retrieving a single language by ID
/// </summary>
public class GetLanguageByIdEndpoint(ISender sender) : Endpoint<GetLanguageByIdRequest, GetLanguageByIdResponse>
{
  public override void Configure()
  {
    Get("/admin/languages/{id}");
    Description(x => x
        .WithName("GetLanguageById")
        .WithTags("Languages")
        .Produces<GetLanguageByIdResponse>(StatusCodes.Status200OK)
        .ProducesProblem(StatusCodes.Status404NotFound)
        .ProducesProblem(StatusCodes.Status500InternalServerError));
  }

  public override async Task HandleAsync(GetLanguageByIdRequest request, CancellationToken ct)
  {
    var query = new GetLanguageByIdQuery(request.Id);
    var result = await sender.Send(query, ct);

    await SendAsync(new GetLanguageByIdResponse(result.Language), cancellation: ct);
  }
}