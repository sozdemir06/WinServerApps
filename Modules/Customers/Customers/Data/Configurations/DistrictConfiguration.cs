using Customers.Districts.Models;

namespace Customers.Data.Configurations;

public class DistrictConfiguration : IEntityTypeConfiguration<District>
{
    public void Configure(EntityTypeBuilder<District> builder)
    {
        builder.HasKey(e => e.Id);

        builder.Property(e => e.Id)
            .ValueGeneratedOnAdd();

        builder.Property(e => e.Name)
            .IsRequired()
            .HasMaxLength(100);

        builder.Property(e => e.StateCode)
            .HasMaxLength(10);

        builder.Property(e => e.StateName)
            .HasMaxLength(100);

        builder.Property(e => e.CountryCode)
            .IsRequired()
            .HasMaxLength(2);

        builder.Property(e => e.CountryName)
            .IsRequired()
            .HasMaxLength(100);

        builder.Property(e => e.WikiDataId)
            .HasMaxLength(20);

        // Configure relationships
        builder.HasOne(e => e.Country)
            .WithMany()
            .HasForeignKey(e => e.CountryId)
            .OnDelete(DeleteBehavior.Cascade);

        builder.HasOne(e => e.City)
            .WithMany(c => c.Districts)
            .HasForeignKey(e => e.CityId)
            .OnDelete(DeleteBehavior.Cascade);

        builder.HasIndex(e => new { e.CountryId, e.CityId, e.Name });

        // Configure audit properties
        builder.Property(e => e.CreatedAt)
            .IsRequired();

        builder.Property(e => e.CreatedBy)
            .IsRequired(false);

        builder.Property(e => e.UpdatedAt)
            .IsRequired(false);

        builder.Property(e => e.ModifiedBy)
            .IsRequired(false);

        // Configure soft delete
        builder.Property(e => e.IsDeleted)
            .HasDefaultValue(false);

        builder.HasQueryFilter(e => !e.IsDeleted);
    }
}