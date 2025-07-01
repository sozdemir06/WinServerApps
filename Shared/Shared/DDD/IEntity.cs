namespace Shared.DDD;

/// <summary>
/// Base interface for domain entities in DDD pattern
/// </summary>
/// <typeparam name="TKey">The type of the entity's identifier</typeparam>
public interface IEntity<TKey>:IEntity
{
  TKey Id { get; }
}

public interface IEntity
{
  string? CreatedBy { get; set; }
  string? ModifiedBy { get; set; }
  DateTime CreatedAt { get;set; }
  DateTime? UpdatedAt { get;set; } 
  bool IsDeleted { get; set;}
  byte[]? RowVersion { get; set;}
}