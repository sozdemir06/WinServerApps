using Customers.Currencies.Models;

namespace Customers.Data.Configurations;

public class CurrencyConfiguration : IEntityTypeConfiguration<Currency>
{
  public void Configure(EntityTypeBuilder<Currency> builder)
  {
    // Primary Key
    builder.HasKey(e => e.Id);

    // Required Properties
    builder.Property(e => e.CurrencyCode)
        .IsRequired()
        .HasMaxLength(10)
        .HasComment("Currency code (e.g., USD, EUR)");

    builder.Property(e => e.CurrencyName)
        .IsRequired()
        .HasMaxLength(100)
        .HasComment("Currency name");

    builder.Property(e => e.ForexBuying)
        .HasPrecision(18, 6)
        .HasComment("Current exchange rate to base currency");

    builder.Property(e => e.ForexSelling)
        .HasPrecision(18, 6)
        .HasComment("Current exchange rate to base currency");

    // Optional Properties
    builder.Property(e => e.BanknoteBuying)
        .HasPrecision(18, 6)
        .HasComment("Current exchange rate to base currency");

    builder.Property(e => e.BanknoteSelling)
        .HasPrecision(18, 6)
        .HasComment("Current exchange rate to base currency");

    builder.Property(e => e.Date)
        .HasComment("Date of the exchange rate");

    // Soft Delete
    builder.Property(e => e.IsDeleted)
        .HasDefaultValue(false)
        .HasComment("Soft delete flag");

    builder.HasQueryFilter(e => !e.IsDeleted);

    // Indexes
    builder.HasIndex(e => e.CurrencyCode)
        .IsUnique();

    builder.HasIndex(e => e.CurrencyCode);
    builder.HasIndex(e => e.CurrencyName);
    builder.HasIndex(e => e.Date);
  }
}