using BitzArt;
using Microsoft.EntityFrameworkCore;

namespace Dberries.Store.Persistence;

public class UsersRepository : RepositoryBase, IUsersRepository
{
    public UsersRepository(AppDbContext db) : base(db)
    {
    }

    public async Task<User?> GetAsync(IFilterSet<User> filterSet)
    {
        return await Db.Set<User>()
            .Apply(filterSet)
            .FirstOrDefaultAsync();
    }

    public async Task<User> GetByExternalIdAsync(Guid id)
    {
        var user = await Db.Set<User>()
            .Where(x => x.ExternalId == id)
            .FirstOrDefaultAsync();

        if (user is null)
            throw ApiException.NotFound($"{nameof(User)} with External '{id}' is not found");

        return user;
    }

    public async Task AddAsync(User user)
    {
        await Db.ThrowIfExistsByExternalIdAsync(typeof(User), user.ExternalId!.Value);
        Db.Add(user);
    }
}