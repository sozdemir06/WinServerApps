using System.Reflection;
using Accounting.Currencies.Models;
using Accounting.ExpensePens.Models;
using Accounting.Languages.Models;
using Accounting.Taxes.Models;
using Accounting.TaxGroups.Models;


namespace Accounting.Data;

public class AccountingDbContext(DbContextOptions<AccountingDbContext> options, IClaimsPrincipalService claimsPrincipalService) : DbContext(options)
{
  private readonly Guid _tenantId = claimsPrincipalService.GetCurrentTenantId();
  public DbSet<OutboxMessage> OutboxMessages => Set<OutboxMessage>();
  public DbSet<AppTenant> AppTenants => Set<AppTenant>();
  public DbSet<Currency> Currencies => Set<Currency>();
  public DbSet<ExpensePen> ExpensePens => Set<ExpensePen>();
  public DbSet<ExpensePenTranslate> ExpensePenTranslates => Set<ExpensePenTranslate>();
  public DbSet<TenantExpensePen> TenantExpensePens => Set<TenantExpensePen>();
  public DbSet<TenantExpensePenTranslate> TenantExpensePenTranslates => Set<TenantExpensePenTranslate>();
  public DbSet<TaxGroup> TaxGroups => Set<TaxGroup>();
  public DbSet<TaxGroupTranslate> TaxGroupTranslates => Set<TaxGroupTranslate>();
  public DbSet<Tax> Taxes => Set<Tax>();
  public DbSet<TaxTranslate> TaxTranslates => Set<TaxTranslate>();
  public DbSet<Language> Languages => Set<Language>();
  public DbSet<TenantTaxGroup> TenantTaxGroups => Set<TenantTaxGroup>();
  public DbSet<TenantTaxGroupTranslate> TenantTaxGroupTranslates => Set<TenantTaxGroupTranslate>();
  public DbSet<TenantTax> TenantTaxes => Set<TenantTax>();
  public DbSet<TenantTaxTranslate> TenantTaxTranslates => Set<TenantTaxTranslate>();

  protected override void OnModelCreating(ModelBuilder modelBuilder)
  {
    modelBuilder.HasDefaultSchema("accounting");
    modelBuilder.ApplyConfigurationsFromAssembly(Assembly.GetExecutingAssembly());
    base.OnModelCreating(modelBuilder);

    modelBuilder.Entity<ExpensePen>().HasQueryFilter(x => !x.IsDeleted);
    modelBuilder.Entity<ExpensePenTranslate>().HasQueryFilter(x => !x.IsDeleted);
    modelBuilder.Entity<TenantExpensePen>().HasQueryFilter(x => !x.IsDeleted && x.TenantId == _tenantId);
    modelBuilder.Entity<TenantExpensePenTranslate>().HasQueryFilter(x => !x.IsDeleted);
    modelBuilder.Entity<TaxGroup>().HasQueryFilter(x => !x.IsDeleted);
    modelBuilder.Entity<TaxGroupTranslate>().HasQueryFilter(x => !x.IsDeleted);
    modelBuilder.Entity<Tax>().HasQueryFilter(x => !x.IsDeleted);
    modelBuilder.Entity<TaxTranslate>().HasQueryFilter(x => !x.IsDeleted);
    modelBuilder.Entity<TenantTaxGroup>().HasQueryFilter(x => !x.IsDeleted && x.TenantId == _tenantId);
    modelBuilder.Entity<TenantTaxGroupTranslate>().HasQueryFilter(x => !x.IsDeleted);
    modelBuilder.Entity<TenantTax>().HasQueryFilter(x => !x.IsDeleted && x.TenantId == _tenantId);
    modelBuilder.Entity<TenantTaxTranslate>().HasQueryFilter(x => !x.IsDeleted);
  }
}