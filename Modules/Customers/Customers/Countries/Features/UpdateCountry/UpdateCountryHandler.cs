using Customers.Countries.Exceptions;


namespace Customers.Countries.Features.UpdateCountry;

public record UpdateCountryCommand(long Id, CountryDto Country) : ICommand<UpdateCountryResult>, ICacheRemovingRequest
{
  public List<string> CacheKeysToRemove => [CacheKeys.Countries];
}

public record UpdateCountryResult(long Id);

public class UpdateCountryCommandValidator : AbstractValidator<UpdateCountryCommand>
{
  public UpdateCountryCommandValidator()
  {
    RuleFor(x => x.Id).GreaterThan(0);
    RuleFor(x => x.Country.Name).NotEmpty().WithMessage("Name is required");
    
  }
}

public class UpdateCountryHandler(
  CustomerDbContext dbContext,
  ILocalizationService localizationService
  ) : ICommandHandler<UpdateCountryCommand, UpdateCountryResult>
{
  public async Task<UpdateCountryResult> Handle(UpdateCountryCommand request, CancellationToken cancellationToken)
  {
    var country = await dbContext.Countries
        .FirstOrDefaultAsync(c => c.Id == request.Id, cancellationToken);

    if (country == null)
      throw new CountryNotFoundException(await localizationService.Translate("NotFound")); 

    country.Update(
        request.Country.Name,
        request.Country.CountryCode,
        request.Country.Latitude,
        request.Country.Longitude,
        request.Country.Iso3,
        request.Country.Iso2,
        request.Country.NumericCode,
        request.Country.PhoneCode,
        request.Country.Capital,
        request.Country.Currency,
        request.Country.CurrencyName,
        request.Country.CurrencySymbol,
        request.Country.Tld,
        request.Country.Native,
        request.Country.Region,
        request.Country.Subregion,
        request.Country.Nationality,
        request.Country.Emoji,
        request.Country.EmojiHtml);

    await dbContext.SaveChangesAsync(cancellationToken);
    return new UpdateCountryResult(country.Id);
  }
}