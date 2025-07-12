
using Shared.DDD;
using WinApps.Modules.Catalog.Catalog.Categories.DomainEvents;
using WinApps.Modules.Catalog.Catalog.Categories.Dtos;

namespace Catalog.Categories.Models
{
    public class AdminCategory : Aggregate<Guid>
    {
        private readonly List<CategoryTranslate> _translates = [];
        public IReadOnlyCollection<CategoryTranslate> Translates => _translates.AsReadOnly();
        public bool IsActive { get; private set; }
        public Guid? ParentId { get; private set; }
        public virtual AdminCategory? Parent { get; private set; }
        public virtual ICollection<AdminCategory> Children { get; private set; } = [];

        // Private constructor for EF Core
        private AdminCategory() { }

        // Aggregate Root'un oluşturulması için fabrika metodu 
        public static async Task<AdminCategory> Create(IEnumerable<CategoryTranslateDto> translations, bool isActive = true, Guid? parentId = null)
        {
            var category = new AdminCategory
            {
                Id = Guid.CreateVersion7(),
                IsActive = isActive,
                ParentId = parentId,
                CreatedAt = DateTime.UtcNow
            };

            // Sadece dolu olan (boş olmayan) çevirileri ekle
            var validTranslations = translations.Where(t => !string.IsNullOrWhiteSpace(t.Name) && t.LanguageId.HasValue);
            foreach (var transDto in validTranslations)
            {
                var newTranslate = CategoryTranslate.Create(transDto.Name, transDto.Description, transDto.LanguageId, category.Id);
                category._translates.Add(newTranslate);
            }

            category.AddDomainEvent(new AdminCategoryCreatedEvent(category));
            return await Task.FromResult(category);
        }

        public void Update(bool isActive, Guid? parentId = null)
        {

            IsActive = isActive;
            ParentId = parentId;
            UpdatedAt = DateTime.UtcNow;

            AddDomainEvent(new AdminCategoryUpdatedEvent(this));
        }

        public void Activate()
        {
            if (IsActive)
                throw new InvalidOperationException("Category is already active.");

            IsActive = true;
            UpdatedAt = DateTime.UtcNow;
            AddDomainEvent(new AdminCategoryActivatedEvent(this));
        }

        public void Deactivate()
        {
            if (!IsActive)
                throw new InvalidOperationException("Category is already inactive.");

            IsActive = false;
            UpdatedAt = DateTime.UtcNow;
            IsDeleted = true;
            AddDomainEvent(new AdminCategoryDeactivatedEvent(this));
        }
    }
}