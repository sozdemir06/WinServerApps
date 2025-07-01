
using Users.Languages.DomainEvents;
using Users.RoleGroups.models;

namespace Users.Languages.Models;

public class Language : Aggregate<Guid>
{
  public string Name { get; private set; } = default!;
  public string Code { get; private set; } = default!;
  public string? Description { get; private set; }
  public bool IsDefault { get; private set; }
  public bool IsActive { get; private set; }

  public ICollection<RoleGroupTranslatate> RoleGroupTranslatates { get; private set; } = [];

  // Private constructor for EF Core
  private Language() { }

  public static Language Create(
      string name,
      string code,
      string? description,
      bool isDefault,
      bool isActive)
  {
    ArgumentException.ThrowIfNullOrEmpty(name);
    ArgumentException.ThrowIfNullOrEmpty(code);

    var language = new Language
    {
      Id = Guid.NewGuid(),
      Name = name,
      Code = code,
      Description = description,
      IsDefault = isDefault,
      IsActive = isActive,
      CreatedAt = DateTime.UtcNow
    };

    language.AddDomainEvent(new LanguageCreatedEvent(language));

    return language;
  }

  public void Update(
      string name,
      string code,
      string? description,
      bool isDefault,
      bool isActive)
  {
    ArgumentException.ThrowIfNullOrEmpty(name);
    ArgumentException.ThrowIfNullOrEmpty(code);

    Name = name;
    Code = code;
    Description = description;
    IsDefault = isDefault;
    IsActive = isActive;

    AddDomainEvent(new LanguageUpdatedEvent(this));
  }

  public void SetAsDefault()
  {
    IsDefault = true;
    AddDomainEvent(new LanguageSetAsDefaultEvent(this));
  }

  public void Activate()
  {
    IsActive = true;
  }

  public void Deactivate()
  {
    if (IsDefault)
    {
      throw new InvalidOperationException("Default language cannot be deactivated");
    }
    IsActive = false;
  }
}