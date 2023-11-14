using BitzArt;

namespace Dberries.Store;

public interface IUsersService
{
    public Task<User> GetAsync(IFilterSet<User> filter);
    public Task<User> AddAsync(User user);
}