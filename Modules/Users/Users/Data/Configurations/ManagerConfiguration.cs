using Users.Managers.Models;

namespace Users.Data.Configurations;

public class ManagerConfiguration() : IEntityTypeConfiguration<Manager>
{
    public void Configure(EntityTypeBuilder<Manager> builder)
    {

        builder.HasKey(m => m.Id);

        // Configure Identity properties
        builder.Property(m => m.UserName)
            .IsRequired()
            .HasMaxLength(256);

        builder.Property(m => m.NormalizedUserName)
            .HasMaxLength(256);

        builder.Property(m => m.Email)
            .IsRequired()
            .HasMaxLength(256);

        builder.Property(m => m.NormalizedEmail)
            .HasMaxLength(256);

        builder.Property(m => m.PhoneNumber)
            .HasMaxLength(20);

        // Configure domain properties
        builder.Property(m => m.IsAdmin)
            .IsRequired()
            .HasDefaultValue(false);

        builder.Property(m => m.IsManager)
            .IsRequired()
            .HasDefaultValue(false);

        builder.Property(m => m.IsActive)
            .IsRequired()
            .HasDefaultValue(true);

        builder.Property(m => m.TenantId)
            .IsRequired(false);

        // Configure audit properties
        builder.Property(m => m.CreatedAt)
            .IsRequired();

        builder.Property(m => m.CreatedBy)
            .IsRequired(false);

        builder.Property(m => m.UpdatedAt)
            .IsRequired(false);

        builder.Property(m => m.ModifiedBy)
            .IsRequired(false);

        // Configure soft delete
        builder.Property(m => m.IsDeleted)
            .IsRequired()
            .HasDefaultValue(false);

        builder.HasQueryFilter(m => !m.IsDeleted);

        // Configure concurrency
        builder.Property(m => m.RowVersion)
            .IsRowVersion()
            .IsConcurrencyToken();

        // Configure indexes
        builder.HasIndex(m => m.NormalizedUserName)
            .IsUnique()
            .HasDatabaseName("UserNameIndex");

        builder.HasIndex(m => m.NormalizedEmail)
            .IsUnique()
            .HasDatabaseName("EmailIndex");

        // Relationships
        builder.HasOne(m => m.Tenant)
            .WithMany(t => t.Managers)
            .HasForeignKey(m => m.TenantId)
            .OnDelete(DeleteBehavior.Cascade);

        builder.HasOne(m => m.Branch)
            .WithMany(b => b.Managers)
            .HasForeignKey(m => m.BranchId)
            .OnDelete(DeleteBehavior.Cascade);
    }
}