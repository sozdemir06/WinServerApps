namespace Customers.Data.Seed;

public class CustomerSeedData(CustomerDbContext context) : IDataSeeder
{
    public async Task SeedAllAsync(IServiceProvider serviceProvider, CancellationToken cancellationToken = default)
    {
        if (!await context.Countries.AnyAsync(cancellationToken))
        {
            var countries = InitialData.GetCountries();
            await context.Countries.AddRangeAsync(countries, cancellationToken);
            await context.SaveChangesAsync(cancellationToken);
        }

        if (!await context.Cities.AnyAsync(cancellationToken) && await context.Countries.AnyAsync(cancellationToken))
        {
            var cities = InitialData.GetCities();
            await context.Cities.AddRangeAsync(cities, cancellationToken);
            await context.SaveChangesAsync(cancellationToken);
        }

        if (!await context.Districts.AnyAsync(cancellationToken) && await context.Cities.AnyAsync(cancellationToken))
        {
            var districts = InitialData.GetDistricts();
            await context.Districts.AddRangeAsync(districts, cancellationToken);
            await context.SaveChangesAsync(cancellationToken);
        }
    }
}