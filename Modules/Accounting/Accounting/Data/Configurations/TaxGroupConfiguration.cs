using Accounting.TaxGroups.Models;


namespace Accounting.Data.Configurations;

public class TaxGroupConfiguration : IEntityTypeConfiguration<TaxGroup>
{
  public void Configure(EntityTypeBuilder<TaxGroup> builder)
  {

    builder.HasKey(x => x.Id);

    builder.Property(x => x.IsActive)
        .IsRequired()
        .HasDefaultValue(true);

    builder.Property(x => x.IsDefault)
        .IsRequired()
        .HasDefaultValue(false);

    // Navigation property
    builder.HasMany(x => x.TaxGroupTranslates)
        .WithOne(x => x.TaxGroup)
        .HasForeignKey(x => x.TaxGroupId)
        .OnDelete(DeleteBehavior.Cascade);

    builder.HasMany(x => x.Taxes)
        .WithOne(x => x.TaxGroup)
        .HasForeignKey(x => x.TaxGroupId)
        .OnDelete(DeleteBehavior.Cascade);
  }
}