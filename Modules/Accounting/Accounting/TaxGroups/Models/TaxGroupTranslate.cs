using Accounting.Languages.Models;

namespace Accounting.TaxGroups.Models;

public class TaxGroupTranslate : Entity<Guid>
{
    public string Name { get; private set; } = string.Empty;
    public string? Description { get; private set; }
    public Guid? LanguageId { get; private set; }
    public Guid TaxGroupId { get; private set; }

    // Navigation Properties
    public Language? Language { get; private set; }
    public TaxGroup TaxGroup { get; private set; } = default!;

    // Private constructor for EF Core
    private TaxGroupTranslate() { }

    public static TaxGroupTranslate Create(
        string name,
        string? description,
        Guid? languageId,
        Guid taxGroupId)
    {
        var taxGroupTranslate = new TaxGroupTranslate
        {
            Id = Guid.CreateVersion7(),
            Name = name,
            Description = description,
            LanguageId = languageId,
            TaxGroupId = taxGroupId,
            CreatedAt = DateTime.UtcNow
        };

        return taxGroupTranslate;
    }

    public void UpdateDetails(string name, string? description)
    {
        Name = name;
        Description = description;
        UpdatedAt = DateTime.UtcNow;
    }
}