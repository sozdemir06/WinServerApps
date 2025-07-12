using Catalog.Languages.Models;

namespace Catalog.Data.Configurations;

public class LanguageConfiguration : IEntityTypeConfiguration<Language>
{
  public void Configure(EntityTypeBuilder<Language> builder)
  {
    builder.HasKey(e => e.Id);

    builder.Property(e => e.Name)
        .IsRequired()
        .HasMaxLength(100);

    builder.Property(e => e.Code)
        .IsRequired()
        .HasMaxLength(10);

    builder.Property(e => e.Description)
        .HasMaxLength(500);

    builder.HasIndex(e => e.Code)
        .IsUnique();

    builder.Property(e => e.RowVersion)
        .IsRowVersion()
        .IsConcurrencyToken();


    // Configure soft delete
    builder.Property(e => e.IsDeleted)
        .HasDefaultValue(false);

  }
}