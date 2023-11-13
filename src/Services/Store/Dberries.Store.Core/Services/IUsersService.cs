using BitzArt;

namespace Dberries.Store;

public interface IUsersService
{
    public Task<User> GetAsync(IFilterSet<User> filterSet);
    public Task<User> AddAsync(IFilterSet<User> filterSet, User user);
}