namespace Accounting.ExpensePens.Models;

public class TenantExpensePen : Aggregate<Guid>
{

  public bool IsActive { get; private set; }
  public bool IsDefault { get; private set; }

  public Guid? TenantId { get; private set; }
  public AppTenant? AppTenant { get; private set; }
  public List<TenantExpensePenTranslate> TenantExpensePenTranslates { get; private set; } = [];

  // Private constructor for EF Core
  private TenantExpensePen() { }

  public static TenantExpensePen Create(Guid? tenantId, bool isActive = true)
  {
    var tenantExpensePen = new TenantExpensePen
    {
      Id = Guid.CreateVersion7(),
      TenantId = tenantId,
      IsActive = isActive,
      CreatedAt = DateTime.UtcNow
    };

    return tenantExpensePen;
  }

  public void Update(bool isActive)
  {
    IsActive = isActive;
  }

  public void Activate()
  {
    if (IsActive)
      throw new InvalidOperationException("TenantExpensePen is already active.");

    IsActive = true;
  }

  public void SetAsDefault()
  {
    IsDefault = true;
  }

  public void UnsetAsDefault()
  {
    IsDefault = false;
  }

  public void Deactivate()
  {
    IsActive = false;
    IsDeleted = true;
  }

  // Translate ekleme metodu
  internal void AddTranslation(string name, string? description, Guid? languageId)
  {
    var translation = TenantExpensePenTranslate.Create(name, description, languageId, Id);
    TenantExpensePenTranslates.Add(translation);
  }

  // Translate güncelleme metodu
  internal void UpdateTranslation(Guid translationId, string name, string? description)
  {
    var translation = TenantExpensePenTranslates.FirstOrDefault(t => t.Id == translationId);
    if (translation != null)
    {
      translation.UpdateDetails(name, description);
    }
  }

  // Translate silme metodu
  internal void RemoveTranslation(Guid translationId)
  {
    var translation = TenantExpensePenTranslates.FirstOrDefault(t => t.Id == translationId);
    if (translation != null)
    {
      TenantExpensePenTranslates.Remove(translation);
    }
  }
}