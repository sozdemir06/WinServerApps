using Customers.Cities.Models;

namespace Customers.Data.Configurations;

public class CityConfiguration : IEntityTypeConfiguration<City>
{
    public void Configure(EntityTypeBuilder<City> builder)
    {
        builder.HasKey(e => e.Id);

        builder.Property(e => e.Id)
            .ValueGeneratedOnAdd();

        builder.Property(e => e.Name)
            .IsRequired()
            .HasMaxLength(150);

        builder.Property(e => e.CountryCode)
            .IsRequired()
            .HasMaxLength(20);

        builder.Property(e => e.CountryName)
            .IsRequired()
            .HasMaxLength(150);

        builder.Property(e => e.StateCode)
            .HasMaxLength(10);

        builder.Property(e => e.Type)
            .HasMaxLength(50);

        // Configure relationships
        builder.HasOne(e => e.Country)
            .WithMany(c => c.Cities)
            .HasForeignKey(e => e.CountryId)
            .OnDelete(DeleteBehavior.Restrict);

        builder.HasIndex(e => new { e.CountryId, e.Name });

        // Configure soft delete
        builder.Property(e => e.IsDeleted)
            .HasDefaultValue(false)
            .HasComment("Soft delete flag");

        builder.HasQueryFilter(e => !e.IsDeleted);
    }
}