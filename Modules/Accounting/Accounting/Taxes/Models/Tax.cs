using Accounting.TaxGroups.Models;

namespace Accounting.Taxes.Models;

public class Tax : Aggregate<Guid>
{
  public decimal Rate { get; private set; }
  public bool IsActive { get; private set; }
  public bool IsDefault { get; private set; }
  public List<TaxTranslate> TaxTranslates { get; private set; } = new();
  public Guid TaxGroupId { get; private set; }
  public TaxGroup TaxGroup { get; private set; } = null!;

  // Private constructor for EF Core
  private Tax() { }

  public static Tax Create(decimal rate, bool isActive = true)
  {
    if (rate < 0 || rate > 100)
      throw new ArgumentException("Tax rate must be between 0 and 100.");

    var tax = new Tax
    {
      Id = Guid.CreateVersion7(),
      Rate = rate,
      IsActive = isActive,
      CreatedAt = DateTime.UtcNow
    };

    return tax;
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
      throw new InvalidOperationException("Tax is already active.");

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

  public void UpdateTaxGroup(Guid taxGroupId)
  {
    TaxGroupId = taxGroupId;
    UpdatedAt = DateTime.UtcNow;
  }

  // Translate ekleme metodu
  internal void AddTranslation(string name, string? description, Guid? languageId)
  {
    var translation = TaxTranslate.Create(name, description, languageId, Id);
    TaxTranslates.Add(translation);
  }

  // Translate güncelleme metodu
  internal void UpdateTranslation(Guid translationId, string name, string? description)
  {
    var translation = TaxTranslates.FirstOrDefault(t => t.Id == translationId);
    if (translation != null)
    {
      translation.UpdateDetails(name, description);
    }
  }

  // Translate silme metodu
  internal void RemoveTranslation(Guid translationId)
  {
    var translation = TaxTranslates.FirstOrDefault(t => t.Id == translationId);
    if (translation != null)
    {
      TaxTranslates.Remove(translation);
    }
  }
}