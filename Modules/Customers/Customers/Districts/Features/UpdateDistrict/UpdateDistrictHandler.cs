using Customers.Cities.Exceptions;
using Customers.Districts.DomainEvents;
using Customers.Districts.Exceptions;


namespace Customers.Districts.Features.UpdateDistrict;

public record UpdateDistrictCommand(long Id, DistrictDto District) : ICommand<UpdateDistrictResult>, ICacheRemovingRequest
{
  public List<string> CacheKeysToRemove => [CacheKeys.Districts];
}

public record UpdateDistrictResult(long Id);

public class UpdateDistrictCommandValidator : AbstractValidator<UpdateDistrictCommand>
{
  public UpdateDistrictCommandValidator()
  {
    RuleFor(x => x.Id).GreaterThan(0).WithMessage("Id is required");
    RuleFor(x => x.District.Name).NotEmpty().WithMessage("Name is required");
    RuleFor(x => x.District.CityId).GreaterThan(0).WithMessage("CityId is required");
  }
}

public class UpdateDistrictHandler(
    CustomerDbContext dbContext,
    IPublisher publisher,
    ILocalizationService localizationService)
    : ICommandHandler<UpdateDistrictCommand, UpdateDistrictResult>
{
  public async Task<UpdateDistrictResult> Handle(UpdateDistrictCommand request, CancellationToken cancellationToken)
  {
    var district = await dbContext.Districts
        .FirstOrDefaultAsync(d => d.Id == request.Id, cancellationToken);
    if (district == null)
    {
      throw new DistrictNotFoundException(await localizationService.Translate("NotFound"));
    }

    var city = await dbContext.Cities
        .AsNoTracking()
        .FirstOrDefaultAsync(c => c.Id == request.District.CityId, cancellationToken);

    if (city == null)
    {
      throw new CityNotFoundException(await localizationService.Translate("CityNotFound"));
    }



    district.Update(
        request.District.Name ?? string.Empty,
        city.Id,
        request.District.StateCode ?? string.Empty,
        request.District.StateName ?? string.Empty,
        city.CountryId,
        request.District.CountryCode ?? string.Empty,
        request.District.CountryName ?? string.Empty,
        request.District.Latitude,
        request.District.Longitude,
        request.District.WikiDataId ?? string.Empty
    );

    await dbContext.SaveChangesAsync(cancellationToken);
    await publisher.Publish(new DistrictUpdatedEvent(district), cancellationToken);

    return new UpdateDistrictResult(district.Id);
  }
}