using Accounting.Languages.Models;
using Accounting.Taxes.Models;

namespace Accounting.TaxGroups.Models
{
    public class TenantTaxGroup : Aggregate<Guid>
    {
        public bool IsActive { get; private set; }
        public bool IsDefault { get; private set; }
        public Guid? TenantId { get; private set; }
        public AppTenant? AppTenant { get; private set; }
        public List<TenantTaxGroupTranslate> TenantTaxGroupTranslates { get; private set; } = new();
        public ICollection<TenantTax> TenantTaxes { get; private set; } = [];

        // Private constructor for EF Core
        private TenantTaxGroup() { }

        public static TenantTaxGroup Create(bool isActive = true, Guid? tenantId = null)
        {
            var tenantTaxGroup = new TenantTaxGroup
            {
                Id = Guid.CreateVersion7(),
                IsActive = isActive,
                TenantId = tenantId,
                CreatedAt = DateTime.UtcNow
            };

            return tenantTaxGroup;
        }

        public void Update(bool isActive)
        {
            IsActive = isActive;
            UpdatedAt = DateTime.UtcNow;
        }

        public void Activate()
        {
            if (IsActive)
                throw new InvalidOperationException("TenantTaxGroup is already active.");

            IsActive = true;
            UpdatedAt = DateTime.UtcNow;
        }

        public void SetAsDefault()
        {
            IsDefault = true;
            UpdatedAt = DateTime.UtcNow;
        }

        public void UnsetAsDefault()
        {
            IsDefault = false;
            UpdatedAt = DateTime.UtcNow;
        }

        public void Deactivate()
        {
            IsActive = false;
            IsDeleted = true;
        }

        // Translate ekleme metodu
        internal void AddTranslation(string name, string? description, Guid? languageId)
        {
            var translation = TenantTaxGroupTranslate.Create(name, description, languageId, Id);
            TenantTaxGroupTranslates.Add(translation);
        }

        // Translate güncelleme metodu
        internal void UpdateTranslation(Guid translationId, string name, string? description)
        {
            var translation = TenantTaxGroupTranslates.FirstOrDefault(t => t.Id == translationId);
            if (translation != null)
            {
                translation.UpdateDetails(name, description);
            }
        }

        // Translate silme metodu
        internal void RemoveTranslation(Guid translationId)
        {
            var translation = TenantTaxGroupTranslates.FirstOrDefault(t => t.Id == translationId);
            if (translation != null)
            {
                TenantTaxGroupTranslates.Remove(translation);
            }
        }
    }
}