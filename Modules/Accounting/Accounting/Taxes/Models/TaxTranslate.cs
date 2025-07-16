using Accounting.Languages.Models;

namespace Accounting.Taxes.Models;

public class TaxTranslate : Entity<Guid>
{
  public string Name { get; private set; } = string.Empty;
  public string? Description { get; private set; }
  public Guid? LanguageId { get; private set; }
  public Guid TaxId { get; private set; }

  // Navigation Properties
  public Language? Language { get; private set; }
  public Tax Tax { get; private set; } = default!;

  // Private constructor for EF Core
  private TaxTranslate() { }

  public static TaxTranslate Create(
      string name,
      string? description,
      Guid? languageId,
      Guid taxId)
  {
    var taxTranslate = new TaxTranslate
    {
      Id = Guid.CreateVersion7(),
      Name = name,
      Description = description,
      LanguageId = languageId,
      TaxId = taxId,
      CreatedAt = DateTime.UtcNow
    };

    return taxTranslate;
  }

  public void UpdateDetails(string name, string? description)
  {
    Name = name;
    Description = description;
    UpdatedAt = DateTime.UtcNow;
  }
}