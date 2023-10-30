namespace Dberries;

public interface IEntityRepository : IRepository
{
    public Task<bool> CheckExistsAsync<T>(Guid id, bool throwException = false) where T : class, IEntity;

    public Task<bool> CheckExistsByExternalIdAsync<TExternalKey>(Type entityType, TExternalKey id,
        bool throwException = false);
}