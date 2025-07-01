using Users.UserRoles.Models;

namespace Users.Data.Configurations;

public class UserRoleConfiguration : IEntityTypeConfiguration<UserRole>
{
  public void Configure(EntityTypeBuilder<UserRole> builder)
  {
    builder.HasKey(x => x.Id);

    builder.Property(x => x.ManagerId)
        .IsRequired();

    builder.Property(x => x.RoleId)
        .IsRequired();

    builder.Property(x => x.IsActive)
        .IsRequired();

    // Navigation Properties
    builder.HasOne(x => x.Manager)
        .WithMany(x => x.UserRoles)
        .HasForeignKey(x => x.ManagerId)
        .OnDelete(DeleteBehavior.Cascade);

    builder.HasOne(x => x.AppRole)
        .WithMany()
        .HasForeignKey(x => x.RoleId)
        .OnDelete(DeleteBehavior.Cascade);

    // Indexes
    builder.HasIndex(x => new { x.ManagerId, x.RoleId })
        .IsUnique();
  }
}