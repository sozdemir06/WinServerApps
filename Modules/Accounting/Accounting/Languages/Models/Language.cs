using Accounting.ExpensePens.Models;
using Accounting.Taxes.Models;
using Accounting.TaxGroups.Models;
using Shared.DDD;

namespace Accounting.Languages.Models;

public class Language : Entity<Guid>
{
  public string Name { get; private set; } = default!;
  public string Code { get; private set; } = default!;
  public string? Description { get; private set; }
  public bool IsDefault { get; private set; }
  public bool IsActive { get; private set; }

  public ICollection<ExpensePenTranslate> ExpensePenTranslates { get; private set; } = [];
  public ICollection<TaxGroupTranslate> TaxGroupTranslates { get; private set; } = [];
  public ICollection<TaxTranslate> TaxTranslates { get; private set; } = [];
  public ICollection<TenantTaxGroupTranslate> TenantTaxGroupTranslates { get; private set; } = [];
  public ICollection<TenantTaxTranslate> TenantTaxTranslates { get; private set; } = [];

  private Language() { } // For EF Core

  public static Language Create(
      Guid id,
      string name,
      string code,
      string? description,
      bool isDefault,
      bool isActive)
  {
    ArgumentException.ThrowIfNullOrEmpty(name);
    ArgumentException.ThrowIfNullOrEmpty(code);

    return new Language
    {
      Id = id,
      Name = name,
      Code = code,
      Description = description,
      IsDefault = isDefault,
      IsActive = isActive,
      CreatedAt = DateTime.UtcNow
    };
  }

  public void Update(
      string name,
      string code,
      string? description,
      bool isDefault,
      bool isActive)
  {
    Name = name;
    Code = code;
    Description = description;
    IsDefault = isDefault;
    IsActive = isActive;
  }

  public void SetAsDefault()
  {
    IsDefault = true;
  }

  public void Activate()
  {
    IsActive = true;
  }

  public void Deactivate()
  {
    IsActive = false;
  }
}