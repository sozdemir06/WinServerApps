

namespace Catalog.AppUnits.Models;

public class AppUnit : Aggregate<Guid>
{
  public bool IsActive { get; private set; }
  public bool IsDefault { get; private set; }
  public MeasureUnitType MeasureUnitType { get; private set; }
  public ICollection<AppUnitTranslate> Translates { get; private set; } = [];

  // Private constructor for EF Core
  private AppUnit() { }

  public static AppUnit Create(
      MeasureUnitType measureUnitType,
      bool isActive = true)
  {
    var appUnit = new AppUnit
    {
      Id = Guid.CreateVersion7(),
      MeasureUnitType = measureUnitType,
      IsActive = isActive,
      CreatedAt = DateTime.UtcNow
    };

    return appUnit;
  }

  public void Update(
      MeasureUnitType measureUnitType,
      bool isActive)
  {
    MeasureUnitType = measureUnitType;
    IsActive = isActive;
    UpdatedAt = DateTime.UtcNow;
  }

  public void Activate()
  {
    if (IsActive)
      throw new InvalidOperationException("AppUnit is already active.");

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
    if (!IsActive)
      throw new InvalidOperationException("AppUnit is already inactive.");

    IsActive = false;
    UpdatedAt = DateTime.UtcNow;
    IsDeleted = true;
  }

  // Translate ekleme metodu
  internal void AddTranslation(string name, string? description, Guid? languageId)
  {
    var translation = AppUnitTranslate.Create(name, description, languageId, Id);
    Translates.Add(translation);
  }

  // Translate güncelleme metodu
  internal void UpdateTranslation(Guid translationId, string name, string? description)
  {
    var translation = Translates.FirstOrDefault(t => t.Id == translationId);
    if (translation != null)
    {
      translation.UpdateDetails(name, description);
    }
  }

  // Translate silme metodu
  internal void RemoveTranslation(Guid translationId)
  {
    var translation = Translates.FirstOrDefault(t => t.Id == translationId);
    if (translation != null)
    {
      Translates.Remove(translation);
    }
  }
}