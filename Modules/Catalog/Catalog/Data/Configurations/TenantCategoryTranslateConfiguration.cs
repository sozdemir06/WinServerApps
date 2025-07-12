

using Catalog.Categories.Models;

namespace Catalog.Data.Configurations;

public class TenantCategoryTranslateConfiguration : IEntityTypeConfiguration<TenantCategoryTranslate>
{
  public void Configure(EntityTypeBuilder<TenantCategoryTranslate> builder)
  {
    builder.HasKey(e => e.Id);

    builder.Property(e => e.Name)
        .IsRequired()
        .HasMaxLength(500);

    builder.Property(e => e.Description)
        .HasMaxLength(1000);

    builder.Property(e => e.LanguageId)
        .IsRequired(false);

    builder.Property(e => e.CategoryId)
        .IsRequired(false);

    // Relationship with Language
    builder.HasOne(e => e.Language)
        .WithMany(e => e.TenantCategoryTranslates)
        .HasForeignKey(e => e.LanguageId)
        .OnDelete(DeleteBehavior.Restrict);

    // Relationship with TenantCategory
    builder.HasOne(e => e.Category)
        .WithMany(e => e.Translates)
        .HasForeignKey(e => e.CategoryId)
        .OnDelete(DeleteBehavior.Cascade);

    builder.Property(e => e.RowVersion)
        .IsRowVersion()
        .IsConcurrencyToken();

    // Configure soft delete
    builder.Property(e => e.IsDeleted)
        .HasDefaultValue(false);
  }
}