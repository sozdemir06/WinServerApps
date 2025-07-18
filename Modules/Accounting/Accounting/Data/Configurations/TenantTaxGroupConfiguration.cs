using Accounting.TaxGroups.Models;

namespace Accounting.Data.Configurations;

public class TenantTaxGroupConfiguration : IEntityTypeConfiguration<TenantTaxGroup>
{
  public void Configure(EntityTypeBuilder<TenantTaxGroup> builder)
  {

    builder.HasKey(x => x.Id);

    builder.Property(x => x.IsActive)
        .IsRequired()
        .HasDefaultValue(true);

    builder.Property(x => x.IsDefault)
        .IsRequired()
        .HasDefaultValue(false);

    builder.Property(x => x.TenantId)
        .IsRequired(false);

    // Navigation property
    builder.HasMany(x => x.TenantTaxGroupTranslates)
        .WithOne(x => x.TenantTaxGroup)
        .HasForeignKey(x => x.TenantTaxGroupId)
        .OnDelete(DeleteBehavior.Cascade);

    builder.HasOne(x => x.AppTenant)
        .WithMany()
        .HasForeignKey(x => x.TenantId)
        .OnDelete(DeleteBehavior.Restrict);

    builder.HasMany(x => x.TenantTaxes)
        .WithOne(x => x.TenantTaxGroup)
        .HasForeignKey(x => x.TenantTaxGroupId)
        .OnDelete(DeleteBehavior.Cascade);

  }
}