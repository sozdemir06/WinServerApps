using Accounting.TaxGroups.Models;

namespace Accounting.Data.Configurations;

public class TaxGroupTranslateConfiguration : IEntityTypeConfiguration<TaxGroupTranslate>
{
  public void Configure(EntityTypeBuilder<TaxGroupTranslate> builder)
  {

    builder.HasKey(x => x.Id);

    builder.Property(x => x.Name)
        .IsRequired()
        .HasMaxLength(100);

    builder.Property(x => x.Description)
        .HasMaxLength(500);

    builder.Property(x => x.LanguageId)
        .IsRequired(false);

    builder.Property(x => x.TaxGroupId)
        .IsRequired();

    builder.HasOne(x => x.Language)
        .WithMany(x => x.TaxGroupTranslates) 
        .HasForeignKey(x => x.LanguageId)
        .OnDelete(DeleteBehavior.Restrict);

    builder.HasOne(x => x.TaxGroup)
        .WithMany(x => x.TaxGroupTranslates)
        .HasForeignKey(x => x.TaxGroupId)
        .OnDelete(DeleteBehavior.Cascade);
  }
}