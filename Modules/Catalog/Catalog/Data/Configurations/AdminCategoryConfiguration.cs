using Catalog.Categories.Models;

namespace Catalog.Data.Configurations;

public class AdminCategoryConfiguration : IEntityTypeConfiguration<AdminCategory>
{
  public void Configure(EntityTypeBuilder<AdminCategory> builder)
  {
    builder.HasKey(e => e.Id);

    builder.Property(e => e.IsActive)
        .IsRequired()
        .HasDefaultValue(true);

    builder.Property(e => e.ParentId)
        .IsRequired(false);

    // Self-referencing relationship for parent-child categories
    builder.HasOne(e => e.Parent)
        .WithMany(e => e.Children)
        .HasForeignKey(e => e.ParentId)
        .OnDelete(DeleteBehavior.Restrict);

    // One-to-many relationship with CategoryTranslate
    builder.HasMany(e => e.Translates)
        .WithOne(e => e.Category)
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