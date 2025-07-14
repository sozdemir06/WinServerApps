using Catalog.Languages.Models;
using Shared.DDD;

namespace Catalog.AppUnits.Models;

public class AppUnitTranslate : Entity<Guid>
{
  public string Name { get; private set; } = default!;
  public string? Description { get; private set; }
  public Guid LanguageId { get; private set; }
  public Guid UnitId { get; private set; }

  // Navigation Properties
  public Language Language { get; private set; } = default!;
  public AppUnit Unit { get; private set; } = default!;

  // Private constructor for EF Core
  private AppUnitTranslate() { }

  public static AppUnitTranslate Create(string name, string? description, Guid? languageId, Guid? unitId)
  {
    ArgumentException.ThrowIfNullOrEmpty(name);

    return new AppUnitTranslate
    {
      Id = Guid.CreateVersion7(),
      Name = name,
      Description = description,
      LanguageId = languageId ?? Guid.Empty,
      UnitId = unitId ?? Guid.Empty,
      CreatedAt = DateTime.UtcNow
    };
  }

  public void UpdateDetails(string name, string? description)
  {
    ArgumentException.ThrowIfNullOrEmpty(name);

    Name = name;
    Description = description;
    UpdatedAt = DateTime.UtcNow;
  }
}