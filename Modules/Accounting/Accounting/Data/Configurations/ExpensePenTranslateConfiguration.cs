using Accounting.ExpensePens.Models;

namespace Accounting.Data.Configurations;

public class ExpensePenTranslateConfiguration : IEntityTypeConfiguration<ExpensePenTranslate>
{
  public void Configure(EntityTypeBuilder<ExpensePenTranslate> builder)
  {

    builder.HasKey(x => x.Id);

    builder.Property(x => x.Name)
        .IsRequired()
        .HasMaxLength(100);

    builder.Property(x => x.Description)
        .HasMaxLength(500);

    builder.Property(x => x.LanguageId)
        .IsRequired(false);

    builder.Property(x => x.ExpensePenId)
        .IsRequired();

    builder.HasOne(x => x.Language)
        .WithMany(x => x.ExpensePenTranslates) 
        .HasForeignKey(x => x.LanguageId)
        .OnDelete(DeleteBehavior.Restrict);

    builder.HasOne(x => x.ExpensePen)
        .WithMany(x => x.ExpensePenTranslates)
        .HasForeignKey(x => x.ExpensePenId)
        .OnDelete(DeleteBehavior.Cascade);
  }
}