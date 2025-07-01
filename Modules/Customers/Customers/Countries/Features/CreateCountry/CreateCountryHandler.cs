


namespace Customers.Countries.Features.CreateCountry;

public record CreateCountryCommand(CountryDto Country) : ICommand<CreateCountryResult>, ICacheRemovingRequest
{
    public List<string> CacheKeysToRemove => [CacheKeys.Countries];
}

public record CreateCountryResult(long Id);

public class CreateCountryCommandValidator : AbstractValidator<CreateCountryCommand>
{
  public CreateCountryCommandValidator()
  {
    RuleFor(x => x.Country.Name).NotEmpty().WithMessage("Name is required");
    RuleFor(x => x.Country.CountryCode).NotEmpty().WithMessage("CountryCode is required");
  }
}

public class CreateCountryHandler (CustomerDbContext dbContext): ICommandHandler<CreateCountryCommand, CreateCountryResult>
{
 
  public async Task<CreateCountryResult> Handle(CreateCountryCommand request, CancellationToken cancellationToken)
  {
    var country = CreateNewCountry(request.Country);
    await dbContext.Countries.AddAsync(country, cancellationToken);
    await dbContext.SaveChangesAsync(cancellationToken);
    return new CreateCountryResult(country.Id);
  }

  private Country CreateNewCountry(CountryDto countryDto)
  {
    return Country.Create(
        countryDto.Id,
        countryDto.Name ?? string.Empty,
        countryDto.CountryCode ?? string.Empty,
        countryDto.Latitude,
        countryDto.Longitude,
        countryDto.Iso3 ?? string.Empty,
        countryDto.Iso2 ?? string.Empty,
        countryDto.NumericCode ?? string.Empty,
        countryDto.PhoneCode ?? string.Empty,
        countryDto.Capital ?? string.Empty,
        countryDto.Currency ?? string.Empty,
        countryDto.CurrencyName ?? string.Empty,
        countryDto.CurrencySymbol ?? string.Empty,
        countryDto.Tld ?? string.Empty,
        countryDto.Native ?? string.Empty,
        countryDto.Region ?? string.Empty,
        countryDto.Subregion ?? string.Empty,
        countryDto.Nationality ?? string.Empty,
        countryDto.Emoji ?? string.Empty,
        countryDto.EmojiHtml ?? string.Empty
    );
  }
}