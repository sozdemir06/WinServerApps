using Customers.Cities.Exceptions;
using Customers.Countries.Exceptions;


namespace Customers.Cities.Features.UpdateCity;

public record UpdateCityCommand(long Id, CityDto City) : ICommand<UpdateCityResult>, ICacheRemovingRequest
{
  public List<string> CacheKeysToRemove => [CacheKeys.Cities];
}

public record UpdateCityResult(long Id);

public class UpdateCityCommandValidator : AbstractValidator<UpdateCityCommand>
{
  public UpdateCityCommandValidator()
  {
    RuleFor(x => x.Id).GreaterThan(0).WithMessage("Id is required");
    RuleFor(x => x.City.Name).NotEmpty().WithMessage("Name is required");
    RuleFor(x => x.City.CountryId).GreaterThan(0).WithMessage("CountryId is required");
  }
}

public class UpdateCityHandler(CustomerDbContext dbContext, ILocalizationService localizationService)
  : ICommandHandler<UpdateCityCommand, UpdateCityResult>
{
  public async Task<UpdateCityResult> Handle(UpdateCityCommand request, CancellationToken cancellationToken)
  {
    var country = await dbContext.Countries.FirstOrDefaultAsync(c => c.Id == request.City.CountryId, cancellationToken);
    if (country == null)
      throw new CountryNotFoundException(await localizationService.Translate("NotFound"));

    var city = await dbContext.Cities
        .FirstOrDefaultAsync(c => c.Id == request.Id, cancellationToken);

    if (city == null)
      throw new CityNotFoundException(await localizationService.Translate("NotFound"));

    city.Update(
        request.City.Name ?? string.Empty,
        request.City.CountryId,
        country.CountryCode!,
        country.Name!,
        request.City.StateCode ?? string.Empty,
        request.City.Type ?? string.Empty,
        request.City.Latitude,
        request.City.Longitude 
    );

    await dbContext.SaveChangesAsync(cancellationToken);
    return new UpdateCityResult(city.Id);
  }
}