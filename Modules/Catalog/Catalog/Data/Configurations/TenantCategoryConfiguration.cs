using Catalog.Categories.Models;

namespace Catalog.Data.Configurations;

public class TenantCategoryConfiguration : IEntityTypeConfiguration<TenantCategory>
{
    public void Configure(EntityTypeBuilder<TenantCategory> builder)
    {
        builder.HasKey(e => e.Id);

        builder.Property(e => e.IsActive)
            .IsRequired()
            .HasDefaultValue(true);

        builder.Property(e => e.TenantId)
        .IsRequired(false);

        builder.Property(e => e.ParentId)
            .IsRequired(false);

        // Relationship with AppTenant
        builder.HasOne(e => e.AppTenant)
            .WithMany(e => e.TenantCategories)
            .HasForeignKey(e => e.TenantId)
            .IsRequired(false)
            .OnDelete(DeleteBehavior.Restrict);

        // Self-referencing relationship for parent-child categories
        builder.HasOne(e => e.Parent)
            .WithMany(e => e.Children)
            .HasForeignKey(e => e.ParentId)
            .IsRequired(false)
            .OnDelete(DeleteBehavior.Restrict);

        // One-to-many relationship with TenantCategoryTranslate
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