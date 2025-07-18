using Accounting.TaxGroups.Models;

namespace Accounting.Data.Configurations;

public class TenantTaxGroupTranslateConfiguration : IEntityTypeConfiguration<TenantTaxGroupTranslate>
{
    public void Configure(EntityTypeBuilder<TenantTaxGroupTranslate> builder)
    {

        builder.HasKey(x => x.Id);

        builder.Property(x => x.Name)
            .IsRequired()
            .HasMaxLength(100);

        builder.Property(x => x.Description)
            .HasMaxLength(500);

        builder.Property(x => x.LanguageId)
            .IsRequired(false);

        builder.Property(x => x.TenantTaxGroupId)
            .IsRequired();

        builder.HasOne(x => x.Language)
            .WithMany(x => x.TenantTaxGroupTranslates)
            .HasForeignKey(x => x.LanguageId)
            .OnDelete(DeleteBehavior.Restrict);

        builder.HasOne(x => x.TenantTaxGroup)
            .WithMany(x => x.TenantTaxGroupTranslates)
            .HasForeignKey(x => x.TenantTaxGroupId)
            .OnDelete(DeleteBehavior.Cascade);
    }
}