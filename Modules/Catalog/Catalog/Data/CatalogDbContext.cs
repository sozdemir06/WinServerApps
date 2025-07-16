using System.Reflection;
using Catalog.AppTenants.Models;
using Catalog.AppUnits.Models;
using Catalog.Currencies.Models;
using Catalog.Languages.Models;


namespace Catalog.Data;

public class CatalogDbContext(
  DbContextOptions<CatalogDbContext> options,
  IClaimsPrincipalService claimsPrincipalService) : DbContext(options)
{
  private readonly Guid? _tenantId = claimsPrincipalService.GetCurrentTenantId();

  public DbSet<OutboxMessage> OutboxMessages => Set<OutboxMessage>();
  public DbSet<AppTenant> AppTenants => Set<AppTenant>();
  public DbSet<Language> Languages => Set<Language>();
  public DbSet<Currency> Currencies => Set<Currency>();
  public DbSet<AdminCategory> AdminCategories => Set<AdminCategory>();
  public DbSet<CategoryTranslate> CategoryTranslates => Set<CategoryTranslate>();
  public DbSet<TenantCategory> TenantCategories => Set<TenantCategory>();
  public DbSet<TenantCategoryTranslate> TenantCategoryTranslates => Set<TenantCategoryTranslate>();
  public DbSet<AppUnit> AppUnits => Set<AppUnit>();
  public DbSet<AppUnitTranslate> AppUnitTranslates => Set<AppUnitTranslate>();

  protected override void OnModelCreating(ModelBuilder modelBuilder)
  {
    modelBuilder.HasDefaultSchema("catalog");
    modelBuilder.ApplyConfigurationsFromAssembly(Assembly.GetExecutingAssembly());
    base.OnModelCreating(modelBuilder);


    modelBuilder.Entity<Language>().HasQueryFilter(e => !e.IsDeleted);
    modelBuilder.Entity<AppTenant>().HasQueryFilter(e => !e.IsDeleted);
    modelBuilder.Entity<Currency>().HasQueryFilter(e => !e.IsDeleted);
    modelBuilder.Entity<AdminCategory>().HasQueryFilter(e => !e.IsDeleted);
    modelBuilder.Entity<CategoryTranslate>().HasQueryFilter(e => !e.IsDeleted);
    modelBuilder.Entity<TenantCategory>().HasQueryFilter(e => !e.IsDeleted && e.TenantId == _tenantId);
    modelBuilder.Entity<AppUnit>().HasQueryFilter(e => !e.IsDeleted);
    modelBuilder.Entity<AppUnitTranslate>().HasQueryFilter(e => !e.IsDeleted);
  }
}