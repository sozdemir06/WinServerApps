using Users.Languages.Constants;
using Users.Languages.DomainExtensions;
using Users.Languages.Dtos;
using Users.Languages.Exceptions;

namespace Users.Languages.Features.GetLanguageById;

public record GetLanguageByIdQuery(Guid Id) : IQuery<GetLanguageByIdResult>, ICachableRequest
{
  public string CacheKey => string.Format(LanguageConstants.CacheKeys.GetLanguageById, Id);
  public string CacheGroupKey => LanguageConstants.CacheKeys.GetLanguages;
  public TimeSpan? CacheExpiration => null;
}

public record GetLanguageByIdResult(LanguageDto Language);

public class GetLanguageByIdHandler(UserDbContext dbContext) : IQueryHandler<GetLanguageByIdQuery, GetLanguageByIdResult>
{
  public async Task<GetLanguageByIdResult> Handle(GetLanguageByIdQuery request, CancellationToken cancellationToken)
  {
    var language = await dbContext.Languages
        .AsNoTracking()
        .FirstOrDefaultAsync(x => x.Id == request.Id, cancellationToken);

    if (language == null)
    {
      throw new LanguageNotFoundException();
    }

    return new GetLanguageByIdResult(language.ToDto());
  }
}