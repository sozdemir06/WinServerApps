using System.Reflection;
using Accounting.Currencies.Models;
using Accounting.ExpensePens.Models;
using Accounting.Languages.Models;


namespace Accounting.Data;

public class AccountingDbContext(DbContextOptions<AccountingDbContext> options) : DbContext(options)
{
  public DbSet<OutboxMessage> OutboxMessages => Set<OutboxMessage>();
  public DbSet<AppTenant> AppTenants => Set<AppTenant>();
  public DbSet<Currency> Currencies => Set<Currency>();
  public DbSet<ExpensePen> ExpensePens => Set<ExpensePen>();
  public DbSet<ExpensePenTranslate> ExpensePenTranslates => Set<ExpensePenTranslate>();
  public DbSet<Language> Languages => Set<Language>();

  protected override void OnModelCreating(ModelBuilder modelBuilder)
  {
    modelBuilder.HasDefaultSchema("accounting");
    modelBuilder.ApplyConfigurationsFromAssembly(Assembly.GetExecutingAssembly());
    base.OnModelCreating(modelBuilder);

    modelBuilder.Entity<ExpensePen>().HasQueryFilter(x => !x.IsDeleted);
    modelBuilder.Entity<ExpensePenTranslate>().HasQueryFilter(x => !x.IsDeleted);
  }
}