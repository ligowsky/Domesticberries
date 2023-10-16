namespace Dberries.Warehouse;

/// <summary>
/// Represents an entity with a unique identifier.
/// </summary>
public interface IEntity
{
    public Guid? Id { get; set; }
}