using Accounting.ExpensePens.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Accounting.Data.Configurations;

public class ExpensePenConfiguration : IEntityTypeConfiguration<ExpensePen>
{
  public void Configure(EntityTypeBuilder<ExpensePen> builder)
  {

    builder.HasKey(x => x.Id);

    builder.Property(x => x.IsActive)
        .IsRequired()
        .HasDefaultValue(true);

    builder.Property(x => x.IsDefault)
        .IsRequired()
        .HasDefaultValue(false);

    // Navigation property
    builder.HasMany(x => x.ExpensePenTranslates)
        .WithOne(x => x.ExpensePen)
        .HasForeignKey(x => x.ExpensePenId)
        .OnDelete(DeleteBehavior.Cascade);
    

    // Indexes
    builder.HasIndex(x => x.IsActive);
    builder.HasIndex(x => x.IsDefault);
  }
}