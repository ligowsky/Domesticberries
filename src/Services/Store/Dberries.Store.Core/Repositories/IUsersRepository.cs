using BitzArt;

namespace Dberries.Store;

public interface IUsersRepository : IRepository
{
    public Task<User?> GetAsync(IFilterSet<User> filter);
    public void Add(User user);
}