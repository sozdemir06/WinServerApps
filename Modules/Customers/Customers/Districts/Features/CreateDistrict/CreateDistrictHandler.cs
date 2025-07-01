using Customers.Cities.Exceptions;
using Customers.Cities.Models;
using Customers.Countries.Exceptions;
using Customers.Districts.DomainEvents;
using Customers.Districts.Models;


namespace Customers.Districts.Features.CreateDistrict;

public record CreateDistrictCommand(DistrictDto District) : ICommand<CreateDistrictResult>, ICacheRemovingRequest
{
  public List<string> CacheKeysToRemove => [CacheKeys.Districts];
}

public record CreateDistrictResult(long Id);

public class CreateDistrictCommandValidator : AbstractValidator<CreateDistrictCommand>
{
  public CreateDistrictCommandValidator()
  {
    RuleFor(x => x.District.Name).NotEmpty().WithMessage("Name is required");
    RuleFor(x => x.District.CityId).GreaterThan(0).WithMessage("CityId is required");
  }
}

public class CreateDistrictHandler(
    CustomerDbContext dbContext,
    IPublisher publisher,
    ILocalizationService localizationService)
    : ICommandHandler<CreateDistrictCommand, CreateDistrictResult>
{
  public async Task<CreateDistrictResult> Handle(CreateDistrictCommand request, CancellationToken cancellationToken)
  {
    var city = await dbContext.Cities
        .FirstOrDefaultAsync(c => c.Id == request.District.CityId, cancellationToken);

    if (city == null)
    {
      throw new CityNotFoundException(await localizationService.Translate("CityNotFound"));
    }
    var country = await dbContext.Countries
        .FirstOrDefaultAsync(c => c.Id == city.CountryId, cancellationToken);
    if (country == null)
    {
      throw new CountryNotFoundException(await localizationService.Translate("CountryNotFound"));
    }

    var district = CreateNewDistrict(request.District, country, city);
    await dbContext.Districts.AddAsync(district, cancellationToken);
    await dbContext.SaveChangesAsync(cancellationToken);

    await publisher.Publish(new DistrictCreatedEvent(city, district), cancellationToken);

    return new CreateDistrictResult(district.Id);
  }

  private District CreateNewDistrict(DistrictDto districtDto, Country country, City city)
  {
    return District.Create(
        districtDto.Name ?? string.Empty,
        districtDto.CityId,
        districtDto.StateCode ?? string.Empty,
        city.Name!,
        country.Id,
        country.CountryCode ?? string.Empty,
        country.Name ?? string.Empty,
        districtDto.Latitude,
        districtDto.Longitude,
        districtDto.WikiDataId ?? string.Empty
    );
  }
}