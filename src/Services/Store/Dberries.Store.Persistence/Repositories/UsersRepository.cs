using BitzArt;
using Microsoft.EntityFrameworkCore;

namespace Dberries.Store.Persistence;

public class UsersRepository : RepositoryBase, IUsersRepository
{
    public UsersRepository(AppDbContext db) : base(db)
    {
    }

    public async Task<User?> GetAsync(IFilterSet<User> filter)
    {
        return await Db.Set<User>()
            .Apply(filter)
            .FirstOrDefaultAsync();
    }

    public async Task AddAsync(User user)
    {
        await Db.ThrowIfExistsByExternalIdAsync(typeof(User), user.ExternalId!.Value);
        Db.Add(user);
    }
}