namespace Dberries;

public interface IEntityWithExternalId<T> : IEntity where T : struct
{
    public T? ExternalId { get; set; }
}