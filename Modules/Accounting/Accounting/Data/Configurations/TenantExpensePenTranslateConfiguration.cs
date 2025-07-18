using Accounting.ExpensePens.Models;

namespace Accounting.Data.Configurations;

public class TenantExpensePenTranslateConfiguration : IEntityTypeConfiguration<TenantExpensePenTranslate>
{
  public void Configure(EntityTypeBuilder<TenantExpensePenTranslate> builder)
  {

    builder.HasKey(x => x.Id);

    builder.Property(x => x.Name)
        .IsRequired()
        .HasMaxLength(100);

    builder.Property(x => x.Description)
        .HasMaxLength(500);

    builder.Property(x => x.LanguageId)
        .IsRequired(false);

    builder.Property(x => x.TenantExpensePenId)
        .IsRequired();

        builder.HasOne(x => x.Language)
        .WithMany(x => x.TenantExpensePenTranslates) 
        .HasForeignKey(x => x.LanguageId)
        .OnDelete(DeleteBehavior.Restrict);

    builder.HasOne(x => x.TenantExpensePen)
        .WithMany(x => x.TenantExpensePenTranslates)
        .HasForeignKey(x => x.TenantExpensePenId)
        .OnDelete(DeleteBehavior.Cascade);
  }
}