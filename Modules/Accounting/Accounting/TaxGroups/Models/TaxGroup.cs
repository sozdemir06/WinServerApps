
using Accounting.Taxes.Models;

namespace Accounting.TaxGroups.Models;

public class TaxGroup : Aggregate<Guid>
{
    public bool IsActive { get; private set; }
    public bool IsDefault { get; private set; }
    public List<TaxGroupTranslate> TaxGroupTranslates { get; private set; } = new();
    public List<Tax> Taxes { get; private set; } = new();

    // Private constructor for EF Core
    private TaxGroup() { }

    public static TaxGroup Create(bool isActive = true)
    {
        var taxGroup = new TaxGroup
        {
            Id = Guid.CreateVersion7(),
            IsActive = isActive,
            CreatedAt = DateTime.UtcNow
        };

        return taxGroup;
    }

    public void Update(bool isActive)
    {
        IsActive = isActive;
        UpdatedAt = DateTime.UtcNow;
    }

    public void Activate()
    {
        if (IsActive)
            throw new InvalidOperationException("TaxGroup is already active.");

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
        var translation = TaxGroupTranslate.Create(name, description, languageId, Id);
        TaxGroupTranslates.Add(translation);
    }

    // Translate güncelleme metodu
    internal void UpdateTranslation(Guid translationId, string name, string? description)
    {
        var translation = TaxGroupTranslates.FirstOrDefault(t => t.Id == translationId);
        if (translation != null)
        {
            translation.UpdateDetails(name, description);
        }
    }

    // Translate silme metodu
    internal void RemoveTranslation(Guid translationId)
    {
        var translation = TaxGroupTranslates.FirstOrDefault(t => t.Id == translationId);
        if (translation != null)
        {
            TaxGroupTranslates.Remove(translation);
        }
    }
}