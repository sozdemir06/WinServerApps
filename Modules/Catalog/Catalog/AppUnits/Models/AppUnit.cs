

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
  }

  public void Activate()
  {
    if (IsActive)
      throw new InvalidOperationException("AppUnit is already active.");

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
    if (!IsActive)
      throw new InvalidOperationException("AppUnit is already inactive.");

    IsActive = false;
    IsDeleted = true; 
  }

}