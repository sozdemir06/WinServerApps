using Accounting.AppTenants.Models;

namespace Accounting.Data.Configurations;

public class AppTenantConfiguration : IEntityTypeConfiguration<AppTenant>
{
  public void Configure(EntityTypeBuilder<AppTenant> builder)
  {
    builder.HasKey(e => e.Id);

    builder.Property(e => e.Name)
        .IsRequired()
        .HasMaxLength(500);

    builder.Property(e => e.Host)
        .IsRequired(false)
        .HasMaxLength(100);

    builder.Property(e => e.TenantCode)
        .IsRequired()
        .HasMaxLength(20);

    builder.Property(e => e.AdminEmail) 
        .IsRequired()
        .HasMaxLength(255);

    builder.Property(e => e.Phone)
        .IsRequired(false)
        .HasMaxLength(20);

    builder.Property(e => e.Description)
        .HasMaxLength(500);

    builder.Property(e => e.TenantType)
        .HasMaxLength(50);

    builder.HasIndex(e => e.TenantCode)
        .IsUnique();

    builder.Property(e => e.RowVersion)
        .IsRowVersion()
        .IsConcurrencyToken();

    // Configure soft delete
    builder.Property(e => e.IsDeleted)
        .HasDefaultValue(false);

    builder.HasQueryFilter(e => !e.IsDeleted);
  }
}