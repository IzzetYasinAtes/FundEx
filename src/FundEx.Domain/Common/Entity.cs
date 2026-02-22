namespace FundEx.Domain.Common;

public abstract class Entity<TId> : IEquatable<Entity<TId>>
    where TId : notnull
{
    public TId Id { get; init; } = default!;

    public DateTime CreatedDate { get; set; } = DateTime.UtcNow;
    public DateTime? UpdatedDate { get; set; }

    protected Entity() { }

    protected Entity(TId id) => Id = id;

    public override bool Equals(object? obj) =>
        obj is Entity<TId> entity && Id.Equals(entity.Id);

    public bool Equals(Entity<TId>? other) =>
        other is not null && Id.Equals(other.Id);

    public override int GetHashCode() => Id.GetHashCode();

    public static bool operator ==(Entity<TId>? left, Entity<TId>? right) =>
        Equals(left, right);

    public static bool operator !=(Entity<TId>? left, Entity<TId>? right) =>
        !Equals(left, right);
}
