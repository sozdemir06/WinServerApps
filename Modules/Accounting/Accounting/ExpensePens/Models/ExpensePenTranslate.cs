
using Accounting.Languages.Models;

namespace Accounting.ExpensePens.Models;

public class ExpensePenTranslate : Entity<Guid>
{
    public string Name { get; private set; } = string.Empty;
    public string? Description { get; private set; }
    public Guid? LanguageId { get; private set; }
    public Guid ExpensePenId { get; private set; }

    // Navigation Properties
    public Language? Language { get; private set; }
    public ExpensePen ExpensePen { get; private set; } = default!;

    // Private constructor for EF Core
    private ExpensePenTranslate() { }

    public static ExpensePenTranslate Create(
        string name,
        string? description,
        Guid? languageId,
        Guid expensePenId)
    {
        var expensePenTranslate = new ExpensePenTranslate
        {
            Id = Guid.CreateVersion7(),
            Name = name,
            Description = description,
            LanguageId = languageId,
            ExpensePenId = expensePenId,
            CreatedAt = DateTime.UtcNow
        };

        return expensePenTranslate;
    }

    public void UpdateDetails(string name, string? description)
    {
        Name = name;
        Description = description;
        UpdatedAt = DateTime.UtcNow;
    }
}