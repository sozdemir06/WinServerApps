using Catalog.AppUnits.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Catalog.Data.Configurations;

public class AppUnitConfiguration : IEntityTypeConfiguration<AppUnit>
{
  public void Configure(EntityTypeBuilder<AppUnit> builder)
  {
    builder.ToTable("AppUnits");

    builder.HasKey(x => x.Id);

    builder.Property(x => x.MeasureUnitType)
         .HasConversion<string>()
         .HasDefaultValue(MeasureUnitType.None)
         .IsRequired();

    builder.Property(x => x.IsDefault)
        .HasDefaultValue(false);

    builder.Property(x => x.IsActive)
        .HasDefaultValue(true);

    builder.HasMany(x => x.Translates)
        .WithOne(x => x.Unit)
        .HasForeignKey(x => x.UnitId)
        .OnDelete(DeleteBehavior.Cascade);
  }
}