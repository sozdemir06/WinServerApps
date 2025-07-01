using Customers.Cities.Models;
using Customers.Currencies.Models;
using Customers.Districts.Models;

namespace Customers.Data;

public class CustomerDbContext(DbContextOptions<CustomerDbContext> options) : DbContext(options)
{
  public DbSet<OutboxMessage> OutboxMessages => Set<OutboxMessage>();
  public DbSet<AppTenant> AppTenants => Set<AppTenant>();
  public DbSet<Country> Countries => Set<Country>();
  public DbSet<City> Cities => Set<City>();
  public DbSet<District> Districts => Set<District>();
  public DbSet<Currency> Currencies => Set<Currency>();

  protected override void OnModelCreating(ModelBuilder modelBuilder)
  {
    modelBuilder.HasDefaultSchema("customers");
    modelBuilder.ApplyConfigurationsFromAssembly(Assembly.GetExecutingAssembly());
    base.OnModelCreating(modelBuilder);


    modelBuilder.Entity<Country>().HasQueryFilter(c => c.IsDeleted == false);
    modelBuilder.Entity<City>().HasQueryFilter(c => c.IsDeleted == false);
    modelBuilder.Entity<District>().HasQueryFilter(d => d.IsDeleted == false);
    modelBuilder.Entity<Currency>().HasQueryFilter(c => c.IsDeleted == false);
  }
}