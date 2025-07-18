using Accounting.Languages.Models;

namespace Accounting.Taxes.Models;

public class TenantTaxTranslate : Entity<Guid>
{
  public string Name { get; private set; } = string.Empty;
  public string? Description { get; private set; }
  public Guid? LanguageId { get; private set; }
  public Guid TenantTaxId { get; private set; }

  // Navigation Properties
  public Language? Language { get; private set; }
  public TenantTax TenantTax { get; private set; } = default!;

  // Private constructor for EF Core
  private TenantTaxTranslate() { }

  public static TenantTaxTranslate Create(
      string name,
      string? description,
      Guid? languageId,
      Guid tenantTaxId)
  {
    var tenantTaxTranslate = new TenantTaxTranslate
    {
      Id = Guid.CreateVersion7(),
      Name = name,
      Description = description,
      LanguageId = languageId,
      TenantTaxId = tenantTaxId,
      CreatedAt = DateTime.UtcNow
    };

    return tenantTaxTranslate;
  }

  public void UpdateDetails(string name, string? description)
  {
    Name = name;
    Description = description;
    UpdatedAt = DateTime.UtcNow;
  }
}