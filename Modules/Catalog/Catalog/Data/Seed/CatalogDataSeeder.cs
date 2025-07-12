
namespace Catalog.Data.Seed;

public class CatalogDataSeeder(CatalogDbContext _dbContext) : IDataSeeder
{
  public async Task SeedAllAsync(IServiceProvider serviceProvider, CancellationToken cancellationToken = default)
  {
    // if (!await _dbContext.AppTenants.AnyAsync(cancellationToken))
    // {
    //   await _dbContext.AppTenants.AddRangeAsync(InitialData.GetAppTenants(), cancellationToken);
    //   await _dbContext.SaveChangesAsync(cancellationToken);
    // }

    // if (!await _dbContext.Languages.AnyAsync(cancellationToken))
    // {
    //   await _dbContext.Languages.AddRangeAsync(InitialData.GetLanguages(), cancellationToken);
    //   await _dbContext.SaveChangesAsync(cancellationToken);
    // }

    // if (!await _dbContext.Currencies.AnyAsync(cancellationToken))
    // {
    //   await _dbContext.Currencies.AddRangeAsync(InitialData.GetCurrencies(), cancellationToken);
    //   await _dbContext.SaveChangesAsync(cancellationToken);
    // }
    await _dbContext.Database.EnsureCreatedAsync(cancellationToken);
  }
}