
using Accounting.Languages.Models;

namespace Accounting.TaxGroups.Models
{
    public class TenantTaxGroupTranslate : Entity<Guid>
    {
        public string Name { get; private set; } = string.Empty;
        public string? Description { get; private set; }
        public Guid? LanguageId { get; private set; }
        public Guid TenantTaxGroupId { get; private set; }

        // Navigation Properties
        public Language? Language { get; private set; }
        public TenantTaxGroup TenantTaxGroup { get; private set; } = default!;

        // Private constructor for EF Core
        private TenantTaxGroupTranslate() { }

        public static TenantTaxGroupTranslate Create(
            string name,
            string? description,
            Guid? languageId,
            Guid tenantTaxGroupId)
        {
            var tenantTaxGroupTranslate = new TenantTaxGroupTranslate
            {
                Id = Guid.CreateVersion7(),
                Name = name,
                Description = description,
                LanguageId = languageId,
                TenantTaxGroupId = tenantTaxGroupId,
                CreatedAt = DateTime.UtcNow
            };

            return tenantTaxGroupTranslate;
        }

        public void UpdateDetails(string name, string? description)
        {
            Name = name;
            Description = description;
            UpdatedAt = DateTime.UtcNow;
        }
    }
}