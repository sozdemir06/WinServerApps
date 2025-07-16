using Accounting.Taxes.Models;

namespace Accounting.Data.Configurations;

public class TaxConfiguration : IEntityTypeConfiguration<Tax>
{
  public void Configure(EntityTypeBuilder<Tax> builder)
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

    builder.Property(x => x.TaxGroupId)
        .IsRequired();

    // Navigation properties
    builder.HasOne(x => x.TaxGroup)
        .WithMany()
        .HasForeignKey(x => x.TaxGroupId)
        .OnDelete(DeleteBehavior.Restrict);

    builder.HasMany(x => x.TaxTranslates)
        .WithOne(x => x.Tax)
        .HasForeignKey(x => x.TaxId)
        .OnDelete(DeleteBehavior.Cascade);

    // Indexes
    builder.HasIndex(x => x.IsActive);
    builder.HasIndex(x => x.IsDefault);
    builder.HasIndex(x => x.Rate);
  }
}