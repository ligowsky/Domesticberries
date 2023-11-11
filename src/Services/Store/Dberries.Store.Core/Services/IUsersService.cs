namespace Dberries.Store;

public interface IUsersService
{
    public Task<User> GetAsync(Guid id);
    public Task<User> AddAsync(User user);
}