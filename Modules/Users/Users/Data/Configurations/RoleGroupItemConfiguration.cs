using Users.RoleGroups.models;

namespace Users.Data.Configurations;

public class RoleGroupItemConfiguration : IEntityTypeConfiguration<RoleGroupItem>
{
  public void Configure(EntityTypeBuilder<RoleGroupItem> builder)
  {
    builder.HasKey(x => x.Id);

    // Configure domain properties
    builder.Property(x => x.RoleGroupId)
        .IsRequired();

    builder.Property(x => x.AppRoleId)
        .IsRequired();

    // Configure concurrency
    builder.Property(x => x.RowVersion)
        .IsRowVersion()
        .IsConcurrencyToken();

    // Configure relationships
    builder.HasOne(x => x.RoleGroup)
        .WithMany(x => x.RoleGroupItems)
        .HasForeignKey(x => x.RoleGroupId)
        .OnDelete(DeleteBehavior.Cascade)
        .IsRequired();

    builder.HasOne(x => x.AppRole)
        .WithMany()
        .HasForeignKey(x => x.AppRoleId)
        .OnDelete(DeleteBehavior.Cascade)
        .IsRequired();

    // Configure indexes
    builder.HasIndex(x => x.Id)
        .IsUnique();

    // Unique constraint: One role per role group
    builder.HasIndex(x => new { x.RoleGroupId, x.AppRoleId })
        .IsUnique()
        .HasDatabaseName("IX_RoleGroupItem_RoleGroupId_AppRoleId");

    // Index for role group queries
    builder.HasIndex(x => x.RoleGroupId)
        .HasDatabaseName("IX_RoleGroupItem_RoleGroupId");

    // Index for app role queries
    builder.HasIndex(x => x.AppRoleId)
        .HasDatabaseName("IX_RoleGroupItem_AppRoleId");
  }
}