using Customers.Countries.Exceptions;


namespace Customers.Countries.Features.DeleteCountry;

public record DeleteCountryCommand(long Id) : ICommand<DeleteCountryResult>, ICacheRemovingRequest
{
  public List<string> CacheKeysToRemove => [CacheKeys.Countries];
}

public record DeleteCountryResult;

public class DeleteCountryCommandValidator : AbstractValidator<DeleteCountryCommand>
{
  public DeleteCountryCommandValidator()
  {
    RuleFor(x => x.Id).GreaterThan(0).WithMessage("Id is required");
  }
}

public class DeleteCountryHandler(CustomerDbContext dbContext,ILocalizationService localizationService) : ICommandHandler<DeleteCountryCommand, DeleteCountryResult>
{


  public async Task<DeleteCountryResult> Handle(DeleteCountryCommand request, CancellationToken cancellationToken)
  {
    var country = await dbContext.Countries
        .Include(c => c.Cities)
        .FirstOrDefaultAsync(c => c.Id == request.Id, cancellationToken);

    if (country == null)
      throw new CountryNotFoundException(await localizationService.Translate("NotFound"));

    // if (country.Cities.Any())
    //   throw new CountryHasCitiesException(await localizationService.Translate("DeleteCannotDelete"));

    country.IsDeleted = true;
    await dbContext.SaveChangesAsync(cancellationToken);
    return new DeleteCountryResult();
  }
}