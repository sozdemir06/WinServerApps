namespace Customers.Data.Configurations;

public class CountryConfiguration : IEntityTypeConfiguration<Country>
{
    public void Configure(EntityTypeBuilder<Country> builder)
    {
        // Primary Key
        builder.HasKey(e => e.Id);

        // Required Properties
        builder.Property(e => e.Name)
            .IsRequired()
            .HasMaxLength(100)
            .HasComment("Country name");

        builder.Property(e => e.CountryCode)
            .IsRequired()
            .HasMaxLength(20)
            .HasComment("Country code (e.g., US, GB)");

        // ISO and Regional Codes
        builder.Property(e => e.Iso3)
            .HasMaxLength(20)
            .HasComment("ISO 3166-1 alpha-3 code");

        builder.Property(e => e.Iso2)
            .HasMaxLength(20)
            .HasComment("ISO 3166-1 alpha-2 code");

        builder.Property(e => e.NumericCode)
            .HasMaxLength(20)
            .HasComment("ISO 3166-1 numeric code");

        builder.Property(e => e.PhoneCode)
            .HasMaxLength(10)
            .HasComment("International dialing code");

        // Geographic Information
        builder.Property(e => e.Type)
            .HasMaxLength(50)
            .HasComment("Country type (e.g., sovereign state)");

        builder.Property(e => e.Capital)
            .HasMaxLength(100)
            .HasComment("Capital city name");

        builder.Property(e => e.Region)
            .HasMaxLength(50)
            .HasComment("Geographic region");

        builder.Property(e => e.Subregion)
            .HasMaxLength(50)
            .HasComment("Geographic subregion");

        // Currency Information
        builder.Property(e => e.Currency)
            .HasMaxLength(20)
            .HasComment("Currency code");

        builder.Property(e => e.CurrencyName)
            .HasMaxLength(50)
            .HasComment("Currency name");

        builder.Property(e => e.CurrencySymbol)
            .HasMaxLength(10)
            .HasComment("Currency symbol");

        // Additional Properties
        builder.Property(e => e.Tld)
            .HasMaxLength(10)
            .HasComment("Top-level domain");

        builder.Property(e => e.Native)
            .HasMaxLength(100)
            .HasComment("Native name");

        builder.Property(e => e.Nationality)
            .HasMaxLength(50)
            .HasComment("Nationality name");

        // Coordinates
        builder.Property(e => e.Latitude)
            .HasPrecision(10, 8)
            .HasComment("Geographic latitude");

        builder.Property(e => e.Longitude)
            .HasPrecision(11, 8)
            .HasComment("Geographic longitude");

        // Flag Emoji
        builder.Property(e => e.Emoji)
            .HasMaxLength(10)
            .HasComment("Country flag emoji");

        builder.Property(e => e.EmojiHtml)
            .HasMaxLength(20)
            .HasComment("HTML code for flag emoji");

        // Soft Delete
        builder.Property(e => e.IsDeleted)
            .HasDefaultValue(false)
            .HasComment("Soft delete flag");

        builder.HasQueryFilter(e => !e.IsDeleted);
    }
}