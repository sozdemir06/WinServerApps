using Accounting.Taxes.Models;

namespace Accounting.Data.Configurations;

public class TaxTranslateConfiguration : IEntityTypeConfiguration<TaxTranslate>
{
  public void Configure(EntityTypeBuilder<TaxTranslate> builder)
  {

    builder.HasKey(x => x.Id);

    builder.Property(x => x.Name)
        .IsRequired()
        .HasMaxLength(100);

    builder.Property(x => x.Description)
        .HasMaxLength(500);

    builder.Property(x => x.LanguageId)
        .IsRequired(false);

    builder.Property(x => x.TaxId)
        .IsRequired();

    builder.HasOne(x => x.Language)
        .WithMany(x => x.TaxTranslates)
        .HasForeignKey(x => x.LanguageId)
        .OnDelete(DeleteBehavior.Restrict);

    builder.HasOne(x => x.Tax)
        .WithMany(x => x.TaxTranslates)
        .HasForeignKey(x => x.TaxId)
        .OnDelete(DeleteBehavior.Cascade);
  }
}