using Accounting.Languages.Models;

namespace Accounting.ExpensePens.Models;

public class TenantExpensePenTranslate : Entity<Guid>
{
  public string Name { get; private set; } = string.Empty;
  public string? Description { get; private set; }
  public Guid? LanguageId { get; private set; }
  public Guid TenantExpensePenId { get; private set; }

  // Navigation Properties
  public Language? Language { get; private set; }
  public TenantExpensePen TenantExpensePen { get; private set; } = default!;

  // Private constructor for EF Core
  private TenantExpensePenTranslate() { }

  public static TenantExpensePenTranslate Create(
      string name,
      string? description,
      Guid? languageId,
      Guid tenantExpensePenId)
  {
    var tenantExpensePenTranslate = new TenantExpensePenTranslate
    {
      Id = Guid.CreateVersion7(),
      Name = name,
      Description = description,
      LanguageId = languageId,
      TenantExpensePenId = tenantExpensePenId,
      CreatedAt = DateTime.UtcNow
    };

    return tenantExpensePenTranslate;
  }

  public void UpdateDetails(string name, string? description)
  {
    Name = name;
    Description = description;
    UpdatedAt = DateTime.UtcNow;
  }
}