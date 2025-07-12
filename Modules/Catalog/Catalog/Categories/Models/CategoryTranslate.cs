
using Catalog.Languages.Models;
using Shared.DDD;

namespace Catalog.Categories.Models
{
    public class CategoryTranslate : Aggregate<Guid>
    {
        public string Name { get; private set; } = default!;
        public string? Description { get; private set; }
        public Language? Language { get; private set; }
        public Guid? LanguageId { get; private set; }
        public AdminCategory? Category { get; private set; }
        public Guid? CategoryId { get; private set; }

        // EF Core için parametresiz constructor
        private CategoryTranslate() { }

        // AdminCategory tarafından çağrılacak fabrika metodu
        internal static CategoryTranslate Create(string name, string? description, Guid? languageId, Guid? categoryId)
        {
            ArgumentException.ThrowIfNullOrEmpty(name);

            var translation = new CategoryTranslate
            {
                Id = Guid.CreateVersion7(),
                Name = name,
                Description = description,
                LanguageId = languageId,
                CategoryId = categoryId,
                CreatedAt = DateTime.UtcNow
            };

            return translation;
        }

        // AdminCategory tarafından çağrılacak güncelleme metodu
        internal void UpdateDetails(string name, string? description)
        {
            ArgumentException.ThrowIfNullOrEmpty(name);

            Name = name;
            Description = description;
            UpdatedAt = DateTime.UtcNow;
        }
    }
}