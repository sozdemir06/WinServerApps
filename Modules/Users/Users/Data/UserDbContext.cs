using Shared.Services.Claims;
using Users.AppRoles.Models;
using Users.Languages.Models;
using Users.Managers.Models;
using Users.RoleGroups.models;
using Users.UserRoles.Models;
using WinApps.Modules.Users.Users.Branches.Models;

namespace Users.Data;

public class UserDbContext : DbContext
{
  private readonly IClaimsPrincipalService? _claimsPrincipalService;
  private readonly Guid? _currentTenantId;
  public UserDbContext(
        DbContextOptions<UserDbContext> options,
        IClaimsPrincipalService? claimsPrincipalService = null)
        : base(options)
  {
    _claimsPrincipalService = claimsPrincipalService;
    _currentTenantId = _claimsPrincipalService?.GetCurrentTenantId() ?? Guid.Empty;
  }

  public DbSet<AppTenant> AppTenants => Set<AppTenant>();
  public DbSet<OutboxMessage> OutboxMessages => Set<OutboxMessage>();
  public DbSet<Manager> Managers => Set<Manager>();
  public DbSet<Branch> Branches => Set<Branch>();
  public DbSet<AppRole> AppRoles => Set<AppRole>();
  public DbSet<UserRole> UserRoles => Set<UserRole>();
  public DbSet<Language> Languages => Set<Language>();
  public DbSet<RoleGroup> RoleGroups => Set<RoleGroup>();
  public DbSet<RoleGroupItem> RoleGroupItems => Set<RoleGroupItem>();
  public DbSet<RoleGroupTranslatate> RoleGroupTranslatates => Set<RoleGroupTranslatate>();

  protected override void OnModelCreating(ModelBuilder modelBuilder)
  {
    base.OnModelCreating(modelBuilder);
    modelBuilder.HasDefaultSchema("users");
    modelBuilder.ApplyConfigurationsFromAssembly(Assembly.GetExecutingAssembly());


    modelBuilder.Entity<AppTenant>().HasQueryFilter(t => t.IsDeleted == false);
    modelBuilder.Entity<Branch>().HasQueryFilter(b => b.TenantId == _currentTenantId && b.IsDeleted == false);
    modelBuilder.Entity<Manager>().HasQueryFilter(m => m.TenantId == _currentTenantId && m.IsDeleted == false);
    modelBuilder.Entity<UserRole>().HasQueryFilter(ur => ur.IsDeleted == false);
    modelBuilder.Entity<Language>().HasQueryFilter(l => l.IsDeleted == false);
    modelBuilder.Entity<RoleGroup>().HasQueryFilter(rg => rg.IsDeleted == false);
    modelBuilder.Entity<RoleGroupItem>().HasQueryFilter(rgi => rgi.IsDeleted == false);
    modelBuilder.Entity<RoleGroupTranslatate>().HasQueryFilter(rgt => rgt.IsDeleted == false);

  }
}