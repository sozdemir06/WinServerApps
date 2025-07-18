using Accounting.ExpensePens.Models;

namespace Accounting.Data.Configurations;

public class TenantExpensePenConfiguration : IEntityTypeConfiguration<TenantExpensePen>
{
  public void Configure(EntityTypeBuilder<TenantExpensePen> builder)
  {

    builder.HasKey(x => x.Id);

    builder.Property(x => x.TenantId)
        .IsRequired();

    builder.Property(x => x.IsActive)
        .IsRequired()
        .HasDefaultValue(true);

    builder.Property(x => x.IsDefault)
        .IsRequired()
        .HasDefaultValue(false);

    // Navigation property
    builder.HasMany(x => x.TenantExpensePenTranslates)
        .WithOne(x => x.TenantExpensePen)
        .HasForeignKey(x => x.TenantExpensePenId)
        .OnDelete(DeleteBehavior.Cascade);

    builder.HasOne(x => x.AppTenant)
        .WithMany(x => x.TenantExpensePens)
        .HasForeignKey(x => x.TenantId)
        .OnDelete(DeleteBehavior.Restrict);

  }
}