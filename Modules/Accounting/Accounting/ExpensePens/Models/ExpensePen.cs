namespace Accounting.ExpensePens.Models;

public class ExpensePen : Aggregate<Guid>
{
    public bool IsActive { get; private set; }
    public bool IsDefault { get; private set; }
    public List<ExpensePenTranslate> ExpensePenTranslates { get; private set; } = new();

    // Private constructor for EF Core
    private ExpensePen() { }

    public static ExpensePen Create(bool isActive = true)
    {
        var expensePen = new ExpensePen
        {
            Id = Guid.CreateVersion7(),
            IsActive = isActive,
            CreatedAt = DateTime.UtcNow
        };

        return expensePen;
    }

    public void Update(bool isActive)
    {
        IsActive = isActive;
        UpdatedAt = DateTime.UtcNow;
    }

    public void Activate()
    {
        if (IsActive)
            throw new InvalidOperationException("ExpensePen is already active.");

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

        IsActive = false;
        IsDeleted = true; 
    }

    // Translate ekleme metodu
    internal void AddTranslation(string name, string? description, Guid? languageId)
    {
        var translation = ExpensePenTranslate.Create(name, description, languageId, Id);
        ExpensePenTranslates.Add(translation);
    }

    // Translate güncelleme metodu
    internal void UpdateTranslation(Guid translationId, string name, string? description)
    {
        var translation = ExpensePenTranslates.FirstOrDefault(t => t.Id == translationId);
        if (translation != null)
        {
            translation.UpdateDetails(name, description);
        }
    }

    // Translate silme metodu
    internal void RemoveTranslation(Guid translationId)
    {
        var translation = ExpensePenTranslates.FirstOrDefault(t => t.Id == translationId);
        if (translation != null)
        {
            ExpensePenTranslates.Remove(translation);
        }
    }
}