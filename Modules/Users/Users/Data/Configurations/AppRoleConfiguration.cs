using Users.AppRoles.Constants;
using Users.AppRoles.Models;

namespace Users.Data.Configurations;

public class AppRoleConfiguration : IEntityTypeConfiguration<AppRole>
{
  public void Configure(EntityTypeBuilder<AppRole> builder)
  {
    
    builder.HasKey(x => x.Id);

    builder.Property(x => x.Name)
        .IsRequired()
        .HasMaxLength(AppRoleConstants.Validation.MaxNameLength);

    builder.Property(x => x.Description)
        .HasMaxLength(AppRoleConstants.Validation.MaxDescriptionLength);

    builder.Property(x => x.RoleLanguageKey)
        .HasConversion<string>();


    builder.Property(x => x.IsActive)
        .IsRequired();

    builder.Property(x => x.CreatedAt)
        .IsRequired();

    // Indexes
    builder.HasIndex(x => x.Name)
        .IsUnique();
  }
}