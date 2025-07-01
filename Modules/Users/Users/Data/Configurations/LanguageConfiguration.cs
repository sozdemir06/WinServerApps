using Users.Languages.Models;

namespace Users.Data.Configurations;

public class LanguageConfiguration : IEntityTypeConfiguration<Language>
{
  public void Configure(EntityTypeBuilder<Language> builder)
  {
    builder.HasKey(x => x.Id);

    builder.Property(x => x.Name)
        .IsRequired()
        .HasMaxLength(100);

    builder.Property(x => x.Code)
        .IsRequired()
        .HasMaxLength(10);

    builder.Property(x => x.Description)
        .HasMaxLength(500);

    builder.Property(x => x.IsDefault)
        .IsRequired();

    builder.Property(x => x.IsActive)
        .IsRequired();

    // Configure audit properties
    builder.Property(x => x.CreatedAt)
        .IsRequired();

    builder.Property(x => x.CreatedBy)
        .IsRequired(false);

    builder.Property(x => x.UpdatedAt)
        .IsRequired(false);

    builder.Property(x => x.ModifiedBy)
        .IsRequired(false);

    // Configure concurrency
    builder.Property(x => x.RowVersion)
        .IsRowVersion()
        .IsConcurrencyToken();

    // Configure unique index for Code
    builder.HasIndex(x => x.Code)
        .IsUnique();
  }
}