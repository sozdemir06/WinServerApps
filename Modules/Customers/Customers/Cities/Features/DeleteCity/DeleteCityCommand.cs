using Customers.Cities.Exceptions;

namespace Customers.Cities.Features.DeleteCity;

public record DeleteCityCommand(long Id) : ICommand<DeleteCityResult>, ICacheRemovingRequest
{
  public List<string> CacheKeysToRemove => [CacheKeys.Cities, CacheKeys.City];
}

public record DeleteCityResult(bool IsSuccess);

public class DeleteCityCommandValidator : AbstractValidator<DeleteCityCommand>
{
  public DeleteCityCommandValidator()
  {
    RuleFor(x => x.Id).GreaterThan(0).WithMessage("Id is required");
  }
}

public class DeleteCityHandler(CustomerDbContext dbContext, ILocalizationService localizationService)
  : ICommandHandler<DeleteCityCommand, DeleteCityResult>
{
  public async Task<DeleteCityResult> Handle(DeleteCityCommand request, CancellationToken cancellationToken)
  {
    var city = await dbContext.Cities
        .Include(c => c.Districts)
        .FirstOrDefaultAsync(c => c.Id == request.Id, cancellationToken);

    if (city == null)
    {
      throw new CityNotFoundException(await localizationService.Translate("NotFound"));
    }

    // Check if city has districts
    if (city.Districts.Any())
    {
      throw new InvalidOperationException(await localizationService.Translate("CityHasDistricts"));
    }

    // Soft delete the city
    city.Delete();

    await dbContext.SaveChangesAsync(cancellationToken);

    return new DeleteCityResult(true);
  }
}