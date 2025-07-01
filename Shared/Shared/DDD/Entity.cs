namespace Shared.DDD;


public abstract class Entity<TKey> : IEntity<TKey>
{
    public TKey Id { get; init; } = default!;
    public string? CreatedBy { get; set; }
    public string? ModifiedBy { get; set; }
    public DateTime CreatedAt { get; set; }
    public DateTime? UpdatedAt { get; set; } = default!;
    public bool IsDeleted { get; set; } = default!;
    public byte[]? RowVersion { get; set; } = default!;

}