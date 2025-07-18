using Accounting.Taxes.Models;

namespace Accounting.Data.Configurations;

public class TenantTaxTranslateConfiguration : IEntityTypeConfiguration<TenantTaxTranslate>
{
    public void Configure(EntityTypeBuilder<TenantTaxTranslate> builder)
    {

        builder.HasKey(x => x.Id);

        builder.Property(x => x.Name)
            .IsRequired()
            .HasMaxLength(100);

        builder.Property(x => x.Description)
            .HasMaxLength(500);

        builder.Property(x => x.LanguageId)
            .IsRequired(false);

        builder.Property(x => x.TenantTaxId)
            .IsRequired();

        builder.HasOne(x => x.Language)
            .WithMany(x => x.TenantTaxTranslates)
            .HasForeignKey(x => x.LanguageId)
            .OnDelete(DeleteBehavior.Restrict);

        builder.HasOne(x => x.TenantTax)
            .WithMany(x => x.TenantTaxTranslates)
            .HasForeignKey(x => x.TenantTaxId)
            .OnDelete(DeleteBehavior.Cascade);
    }
}