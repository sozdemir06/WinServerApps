using Users.RoleGroups.models;

namespace Users.Data.Configurations;

public class RoleGroupTranslatateConfiguration : IEntityTypeConfiguration<RoleGroupTranslatate>
{
    public void Configure(EntityTypeBuilder<RoleGroupTranslatate> builder)
    {
        builder.HasKey(x => x.Id);

        // Configure domain properties
        builder.Property(x => x.Name)
            .IsRequired()
            .HasMaxLength(200);

        builder.Property(x => x.Description)
            .HasMaxLength(500);

        builder.Property(x => x.LanguageId)
            .IsRequired(false);

        builder.Property(x => x.RoleGroupId)
            .IsRequired(false);

        // Configure concurrency
        builder.Property(x => x.RowVersion)
            .IsRowVersion()
            .IsConcurrencyToken();

        // Configure relationships
        builder.HasOne(x => x.RoleGroup)
            .WithMany(x => x.RoleGroupTranslatates)
            .HasForeignKey(x => x.RoleGroupId)
            .OnDelete(DeleteBehavior.Cascade)
            .IsRequired(false);

        // Configure indexes
        builder.HasIndex(x => x.Id)
            .IsUnique();

        // Unique constraint: One translation per language per role group
        builder.HasIndex(x => new { x.RoleGroupId, x.LanguageId })
            .IsUnique()
            .HasDatabaseName("IX_RoleGroupTranslatate_RoleGroupId_LanguageId");

        // Index for language queries
        builder.HasIndex(x => x.LanguageId)
            .HasDatabaseName("IX_RoleGroupTranslatate_LanguageId");
    }
}