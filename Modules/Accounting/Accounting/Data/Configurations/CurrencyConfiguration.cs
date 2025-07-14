using Accounting.Currencies.Models;

namespace Accounting.Data.Configurations;

public class CurrencyConfiguration : IEntityTypeConfiguration<Currency>
{
  public void Configure(EntityTypeBuilder<Currency> builder)
  {
    builder.HasKey(e => e.Id);

    builder.Property(e => e.CurrencyCode)
        .IsRequired(false)
        .HasMaxLength(10);

    builder.Property(e => e.CurrencyName)
        .IsRequired(false)
        .HasMaxLength(100);

    builder.Property(e => e.ForexBuying)
        .HasPrecision(18, 4);

    builder.Property(e => e.ForexSelling)
        .HasPrecision(18, 4);

    builder.Property(e => e.BanknoteBuying)
        .HasPrecision(18, 4);

    builder.Property(e => e.BanknoteSelling)
        .HasPrecision(18, 4);

    builder.HasIndex(e => e.CurrencyCode)
        .IsUnique();

    builder.Property(e => e.RowVersion)
        .IsRowVersion()
        .IsConcurrencyToken();

    // Configure soft delete
    builder.Property(e => e.IsDeleted)
        .HasDefaultValue(false);
  }
}