using Accounting.Taxes.Models;


namespace Accounting.Data.Configurations;

public class TenantTaxConfiguration : IEntityTypeConfiguration<TenantTax>
{
  public void Configure(EntityTypeBuilder<TenantTax> builder)
  {

    builder.HasKey(x => x.Id);

    builder.Property(x => x.Rate)
        .IsRequired()
        .HasPrecision(5, 2);

    builder.Property(x => x.IsActive)
        .IsRequired()
        .HasDefaultValue(true);

    builder.Property(x => x.IsDefault)
        .IsRequired()
        .HasDefaultValue(false);

    builder.Property(x => x.TenantTaxGroupId)
        .IsRequired();

    builder.Property(x => x.TenantId)
        .IsRequired(false);

    // Navigation properties
    builder.HasOne(x => x.TenantTaxGroup)
        .WithMany(x => x.TenantTaxes)
        .HasForeignKey(x => x.TenantTaxGroupId)
        .OnDelete(DeleteBehavior.Restrict);

    builder.HasOne(x => x.AppTenant)
        .WithMany(x => x.TenantTaxes)
        .HasForeignKey(x => x.TenantId)
        .OnDelete(DeleteBehavior.Restrict);

    builder.HasMany(x => x.TenantTaxTranslates)
        .WithOne(x => x.TenantTax)
        .HasForeignKey(x => x.TenantTaxId)
        .OnDelete(DeleteBehavior.Cascade);
  }
}