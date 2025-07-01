using Users.Languages.Dtos;
using Users.Languages.QueryParams;

namespace Users.Languages.Features.GetLanguages;

public record GetLanguagesQuery(GetLanguagesQueryParams QueryParams) : IQuery<GetLanguagesResult>, ICachableRequest
{
  public string CacheKey => CacheKeyGenerator.GenerateKey(CacheKeys.Languages,QueryParams);
  public string CacheGroupKey => CacheKeys.Languages;
  public TimeSpan? CacheExpiration => null;
}

public record GetLanguagesResult(IEnumerable<LanguageDto> Languages, PaginationMetaData MetaData);

public class GetLanguagesHandler(UserDbContext dbContext) : IQueryHandler<GetLanguagesQuery, GetLanguagesResult>
{
  public async Task<GetLanguagesResult> Handle(GetLanguagesQuery request, CancellationToken cancellationToken)
  {


    var languages = await dbContext.Languages.ToListAsync(cancellationToken); 

    return new GetLanguagesResult(languages.Adapt<IEnumerable<LanguageDto>>(), new PaginationMetaData());
  }
}