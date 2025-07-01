using Customers.Cities.Models;
using Customers.Countries.Exceptions;

namespace Customers.Cities.Features.CreateCity;

public record CreateCityCommand(CityDto City) : ICommand<CreateCityResult>, ICacheRemovingRequest
{
  public List<string> CacheKeysToRemove => [CacheKeys.Cities];
}

public record CreateCityResult(long Id);

public class CreateCityCommandValidator : AbstractValidator<CreateCityCommand>
{
  public CreateCityCommandValidator()
  {
    RuleFor(x => x.City.Name).NotEmpty().WithMessage("Name is required");
    RuleFor(x => x.City.CountryId).GreaterThan(0).WithMessage("CountryId is required");
  }
}

public class CreateCityHandler(CustomerDbContext dbContext) : ICommandHandler<CreateCityCommand, CreateCityResult>
{
  public async Task<CreateCityResult> Handle(CreateCityCommand request, CancellationToken cancellationToken)
  {
    var country = await dbContext.Countries.FindAsync(request.City.CountryId, cancellationToken);
    if (country == null)
    {
      throw new CountryNotFoundException(request.City.CountryId.ToString());
    }

    var city = CreateNewCity(request.City, country);
    await dbContext.Cities.AddAsync(city, cancellationToken);
    await dbContext.SaveChangesAsync(cancellationToken);
    return new CreateCityResult(city.Id);
  }

  private City CreateNewCity(CityDto cityDto, Country country)
  {
    return City.Create(
        cityDto.Name ?? string.Empty,
        cityDto.CountryId,
        country.CountryCode ?? string.Empty,
        country.Name ?? string.Empty,
        cityDto.StateCode ?? string.Empty,
        cityDto.Type ?? string.Empty,
        cityDto.Latitude,
        cityDto.Longitude
    );
  }
}