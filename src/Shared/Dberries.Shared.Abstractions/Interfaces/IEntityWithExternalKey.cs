namespace Dberries;

public interface IEntityWithExternalKey<T> : IEntity
{
    public T? ExternalId { get; set; }
}