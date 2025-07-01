using Customers.Countries.Exceptions;


namespace Customers.Countries.Features.GetCountryById;

public record GetCountryByIdQuery(long Id) : IQuery<GetCountryByIdResult>;

public record GetCountryByIdResult(CountryDto Country);

public class GetCountryByIdHandler (CustomerDbContext dbContext,ILocalizationService localizationService): IQueryHandler<GetCountryByIdQuery, GetCountryByIdResult>
{

  public async Task<GetCountryByIdResult> Handle(GetCountryByIdQuery request, CancellationToken cancellationToken)
  {
    var country = await dbContext.Countries
        .Include(c => c.Cities)
        .FirstOrDefaultAsync(c => c.Id == request.Id, cancellationToken);

    if (country == null)
      throw new CountryNotFoundException(await localizationService.Translate("NotFound"));

    var countryDto = country.Adapt<CountryDto>();

    return new GetCountryByIdResult(countryDto);
  }
}