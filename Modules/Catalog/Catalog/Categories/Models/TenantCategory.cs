using Catalog.AppTenants.Models;
using WinApps.Modules.Catalog.Catalog.Categories.DomainEvents;
using WinApps.Modules.Catalog.Catalog.Categories.Dtos;

namespace Catalog.Categories.Models
{
  public class TenantCategory : Aggregate<Guid>
  {
    private readonly List<TenantCategoryTranslate> _translates = [];
    public IReadOnlyCollection<TenantCategoryTranslate> Translates => _translates.AsReadOnly();
    public bool IsActive { get; private set; }
    public Guid? TenantId { get; private set; }
    public AppTenant? AppTenant { get; private set; } = default!;
    public Guid? ParentId { get; private set; }
    public virtual TenantCategory? Parent { get; private set; }
    public virtual ICollection<TenantCategory> Children { get; private set; } = [];

    // Private constructor for EF Core
    private TenantCategory() { }

    // Aggregate Root'un oluşturulması için fabrika metodu
    public static async Task<TenantCategory> Create(IEnumerable<TenantCategoryTranslateDto> translations, Guid tenantId, bool isActive = true, Guid? parentId = null)
    {
      var category = new TenantCategory
      {
        Id = Guid.CreateVersion7(),
        IsActive = isActive,
        TenantId = tenantId,
        ParentId = parentId,
        CreatedAt = DateTime.UtcNow
      };

      // Sadece dolu olan (boş olmayan) çevirileri ekle
      var validTranslations = translations.Where(t => !string.IsNullOrWhiteSpace(t.Name) && t.LanguageId.HasValue);
      foreach (var transDto in validTranslations)
      {
        var newTranslate = TenantCategoryTranslate.Create(transDto.Name, transDto.Description, transDto.LanguageId, category.Id);
        category._translates.Add(newTranslate);
      }

      category.AddDomainEvent(new TenantCategoryCreatedEvent(category));
      return await Task.FromResult(category);
    }

    public void Update(bool isActive, Guid? parentId = null)
    {

      IsActive = isActive;
      ParentId = parentId;
      UpdatedAt = DateTime.UtcNow;

      AddDomainEvent(new TenantCategoryUpdatedEvent(this));
    }

    public void Activate()
    {
      if (IsActive)
        throw new InvalidOperationException("Category is already active.");

      IsActive = true;
      UpdatedAt = DateTime.UtcNow;
      AddDomainEvent(new TenantCategoryActivatedEvent(this));
    }

    public void Deactivate()
    {
      if (!IsActive)
        throw new InvalidOperationException("Category is already inactive.");

      IsActive = false;
      UpdatedAt = DateTime.UtcNow;
      IsDeleted = true;
      AddDomainEvent(new TenantCategoryDeactivatedEvent(this));
    }
  }
}