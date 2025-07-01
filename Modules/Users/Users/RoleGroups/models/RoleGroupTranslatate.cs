using Users.Languages.Models;

namespace Users.RoleGroups.models
{
  public class RoleGroupTranslatate : Aggregate<Guid>
  {
    public string Name { get; private set; } = default!;
    public string? Description { get; private set; }
    public Language? Language { get; private set; }
    public Guid? LanguageId { get; private set; }
    public RoleGroup? RoleGroup { get; private set; }
    public Guid? RoleGroupId { get; private set; }

    // EF Core için parametresiz constructor
    private RoleGroupTranslatate() { }

    // RoleGroup tarafından çağrılacak fabrika metodu
    internal static RoleGroupTranslatate Create(string name, string? description, Guid? languageId, Guid? roleGroupId)
    {
      ArgumentException.ThrowIfNullOrEmpty(name);

      var translation = new RoleGroupTranslatate
      {
        Id = Guid.CreateVersion7(),
        Name = name,
        Description = description,
        LanguageId = languageId,
        RoleGroupId = roleGroupId,
        CreatedAt = DateTime.UtcNow
      };

      return translation;
    }

    // RoleGroup tarafından çağrılacak güncelleme metodu
    internal void UpdateDetails(string name, string? description)
    {
      ArgumentException.ThrowIfNullOrEmpty(name);

      Name = name;
      Description = description;
      UpdatedAt = DateTime.UtcNow;
    }
  }
}