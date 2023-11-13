using BitzArt;

namespace Dberries.Store;

public interface IUsersRepository : IRepository
{
    public Task<User?> GetAsync(IFilterSet<User> filterSet);
    public Task AddAsync(User user);
}