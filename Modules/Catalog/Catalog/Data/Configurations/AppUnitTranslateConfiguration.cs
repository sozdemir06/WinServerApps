using Catalog.AppUnits.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Catalog.Data.Configurations;

public class AppUnitTranslateConfiguration : IEntityTypeConfiguration<AppUnitTranslate>
{
  public void Configure(EntityTypeBuilder<AppUnitTranslate> builder)
  {

    builder.HasKey(x => x.Id);

    builder.Property(x => x.Name)
        .HasMaxLength(100)
        .IsRequired();

    builder.Property(x => x.Description)
        .HasMaxLength(500)
        .IsRequired(false);

    builder.Property(x => x.LanguageId)
        .IsRequired();

    builder.Property(x => x.UnitId)
        .IsRequired();

    // Relationships
    builder.HasOne(x => x.Language)
        .WithMany(x => x.AppUnitTranslates)
        .HasForeignKey(x => x.LanguageId)
        .OnDelete(DeleteBehavior.Cascade);

    builder.HasOne(x => x.Unit)
        .WithMany(x => x.Translates)
        .HasForeignKey(x => x.UnitId)
        .OnDelete(DeleteBehavior.Cascade);

    // Composite unique index for UnitId and LanguageId
    builder.HasIndex(x => new { x.UnitId, x.LanguageId })
        .IsUnique();

    // Indexes
    builder.HasIndex(x => x.LanguageId);
    builder.HasIndex(x => x.UnitId);
  }
}