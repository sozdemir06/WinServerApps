using Users.RoleGroups.Enums;
using Users.RoleGroups.models;

namespace Users.Data.Configurations;

public class RoleGroupConfiguration : IEntityTypeConfiguration<RoleGroup>
{
  public void Configure(EntityTypeBuilder<RoleGroup> builder)
  {
    builder.HasKey(x => x.Id);


    // Configure concurrency
    builder.Property(x => x.RowVersion)
        .IsRowVersion()
        .IsConcurrencyToken();

    builder.Property(x => x.ViewPermission)
        .HasConversion<string>()
        .HasDefaultValue(RoleGroupViewPermission.Admin)
        .IsRequired();

    // Configure relationships
    builder.HasMany(x => x.RoleGroupTranslatates)
        .WithOne(x => x.RoleGroup)
        .HasForeignKey(x => x.RoleGroupId)
        .OnDelete(DeleteBehavior.Cascade)
        .IsRequired(false);

    // Configure indexes
    builder.HasIndex(x => x.Id)
        .IsUnique();
  }
}