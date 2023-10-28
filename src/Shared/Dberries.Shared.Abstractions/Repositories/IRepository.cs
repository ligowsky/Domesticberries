namespace Dberries;

public interface IRepository
{
    public Task<bool> CheckExistsAsync<T>(Guid id, bool throwException) where T : class, IEntity;
    public Task<bool> CheckExistsByExternalIdAsync<T>(Guid id, bool throwException) where T : class, IExternalId;
    public Task<int> SaveChangesAsync();
}