using Catalog.Languages.Models;
using Shared.DDD;

namespace Catalog.Categories.Models
{
  public class TenantCategoryTranslate : Aggregate<Guid>
  {
    public string Name { get; private set; } = default!;
    public string? Description { get; private set; }
    public Language? Language { get; private set; }
    public Guid? LanguageId { get; private set; }
    public TenantCategory? Category { get; private set; }
    public Guid? CategoryId { get; private set; }

    // EF Core için parametresiz constructor
    private TenantCategoryTranslate() { }

    // TenantCategory tarafından çağrılacak fabrika metodu
    internal static TenantCategoryTranslate Create(string name, string? description, Guid? languageId, Guid? categoryId)
    {
      ArgumentException.ThrowIfNullOrEmpty(name);

      var translation = new TenantCategoryTranslate
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

    // TenantCategory tarafından çağrılacak güncelleme metodu
    internal void UpdateDetails(string name, string? description)
    {
      ArgumentException.ThrowIfNullOrEmpty(name);

      Name = name;
      Description = description;
      UpdatedAt = DateTime.UtcNow;
    }
  }
}