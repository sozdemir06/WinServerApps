using FastEndpoints;
using Users.Languages.Dtos;
using Users.Languages.QueryParams;

namespace Users.Languages.Features.GetLanguages;

public record GetLanguagesRequest() : GetLanguagesQueryParams;
public record GetLanguagesResponse(IEnumerable<LanguageDto> Languages, PaginationMetaData MetaData);

/// <summary>
/// Endpoint for retrieving a list of languages
/// </summary>
public class GetLanguagesEndpoint(ISender sender) : Endpoint<GetLanguagesRequest, GetLanguagesResponse>
{
  public override void Configure()
  {
    Get("/admin/languages");
    Description(x => x
        .WithName("GetLanguages")
        .WithTags("Languages")
        .Produces<GetLanguagesResponse>(StatusCodes.Status200OK)
        .ProducesProblem(StatusCodes.Status500InternalServerError));
  }

  public override async Task HandleAsync(GetLanguagesRequest request, CancellationToken ct)
  {
    var query = new GetLanguagesQuery(request);
    var result = await sender.Send(query, ct);

    await SendAsync(new GetLanguagesResponse(result.Languages, result.MetaData), cancellation: ct);
  }
}