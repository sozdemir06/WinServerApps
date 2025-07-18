using Accounting.TaxGroups.Models;

namespace Accounting.Taxes.Models;

public class TenantTax : Aggregate<Guid>
{
  public decimal Rate { get; private set; }
  public bool IsActive { get; private set; }
  public bool IsDefault { get; private set; }
  public ICollection<TenantTaxTranslate> TenantTaxTranslates { get; private set; } = [];
  public Guid TenantTaxGroupId { get; private set; }
  public TenantTaxGroup TenantTaxGroup { get; private set; } = null!;
  public Guid? TenantId { get; private set; }
  public AppTenant? AppTenant { get; private set; }

  // Private constructor for EF Core
  private TenantTax() { }

  public static TenantTax Create(decimal rate, bool isActive = true, Guid? tenantId = null)
  {
    if (rate < 0 || rate > 100)
      throw new ArgumentException("Tax rate must be between 0 and 100.");

    var tenantTax = new TenantTax
    {
      Id = Guid.CreateVersion7(),
      Rate = rate,
      IsActive = isActive,
      TenantId = tenantId,
      CreatedAt = DateTime.UtcNow
    };

    return tenantTax;
  }

  public void Update(decimal rate, bool isActive)
  {
    if (rate < 0 || rate > 100)
      throw new ArgumentException("Tax rate must be between 0 and 100.");

    Rate = rate;
    IsActive = isActive;
    UpdatedAt = DateTime.UtcNow;
  }

  public void Activate()
  {
    if (IsActive)
      throw new InvalidOperationException("TenantTax is already active.");

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

  public void UpdateTenantTaxGroup(Guid tenantTaxGroupId)
  {
    TenantTaxGroupId = tenantTaxGroupId;
    UpdatedAt = DateTime.UtcNow;
  }

  // Translate ekleme metodu
  internal void AddTranslation(string name, string? description, Guid? languageId)
  {
    var translation = TenantTaxTranslate.Create(name, description, languageId, Id);
    TenantTaxTranslates.Add(translation);
  }

  // Translate güncelleme metodu
  internal void UpdateTranslation(Guid translationId, string name, string? description)
  {
    var translation = TenantTaxTranslates.FirstOrDefault(t => t.Id == translationId);
    if (translation != null)
    {
      translation.UpdateDetails(name, description);
    }
  }

  // Translate silme metodu
  internal void RemoveTranslation(Guid translationId)
  {
    var translation = TenantTaxTranslates.FirstOrDefault(t => t.Id == translationId);
    if (translation != null)
    {
      TenantTaxTranslates.Remove(translation);
    }
  }
}