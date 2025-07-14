using System.Reflection;
using Accounting.AppTenants.Models;
using Accounting.Currencies.Models;
using Accounting.Languages.Models;
using Shared.Messages.Models;

namespace Accounting.Data;

public class AccountingDbContext(DbContextOptions<AccountingDbContext> options) : DbContext(options)
{
  public DbSet<OutboxMessage> OutboxMessages => Set<OutboxMessage>();
  public DbSet<AppTenant> AppTenants => Set<AppTenant>();
  public DbSet<Currency> Currencies => Set<Currency>();
  public DbSet<Language> Languages => Set<Language>();

  protected override void OnModelCreating(ModelBuilder modelBuilder)
  {
    modelBuilder.HasDefaultSchema("accounting");
    modelBuilder.ApplyConfigurationsFromAssembly(Assembly.GetExecutingAssembly());
    base.OnModelCreating(modelBuilder);
  }
}