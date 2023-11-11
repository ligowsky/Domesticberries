namespace Dberries.Store;

public interface IUsersRepository : IRepository
{
    public Task<User> GetAsync(Guid id);
    public Task<User> GetByExternalIdAsync(Guid id);
    public Task<User> AddAsync(User user);
}