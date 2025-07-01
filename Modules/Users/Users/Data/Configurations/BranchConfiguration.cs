
using WinApps.Modules.Users.Users.Branches.Models;

namespace WinApps.Modules.Users.Users.Data.Configurations;

public class BranchConfiguration() : IEntityTypeConfiguration<Branch>
{
  

    public void Configure(EntityTypeBuilder<Branch> builder)
  {

    builder.HasKey(x => x.Id);

    builder.Property(x => x.Name)
        .IsRequired()
        .HasMaxLength(140);

    builder.Property(x => x.Code)
        .IsRequired()
        .HasMaxLength(20);

    builder.Property(x => x.Address)
        .HasMaxLength(200);

    builder.Property(x => x.Phone)
        .HasMaxLength(20);

    builder.Property(x => x.Email)
        .HasMaxLength(100);

    builder.Property(x => x.Description)
        .HasMaxLength(500);

    builder.Property(x => x.IsActive)
        .IsRequired()
        .HasDefaultValue(true);

    builder.Property(x => x.TenantId)
        .IsRequired();

    // Audit fields
    builder.Property(x => x.CreatedAt)
        .IsRequired();

    builder.Property(x => x.UpdatedAt);

    builder.Property(x => x.CreatedBy);

    builder.Property(x => x.ModifiedBy);

    // Relationships
    builder.HasOne(x => x.AppTenant)
        .WithMany(x => x.Branches) 
        .HasForeignKey(x => x.TenantId)
        .OnDelete(DeleteBehavior.Cascade);

    builder.HasMany(x => x.Managers)
        .WithOne(x => x.Branch)
        .HasForeignKey(x => x.BranchId)
        .OnDelete(DeleteBehavior.Cascade);



  }
}