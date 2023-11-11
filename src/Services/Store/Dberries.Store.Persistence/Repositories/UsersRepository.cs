using BitzArt;
using Microsoft.EntityFrameworkCore;

namespace Dberries.Store.Persistence;

public class UsersRepository : RepositoryBase, IUsersRepository
{
    public UsersRepository(AppDbContext db) : base(db)
    {
    }

    public async Task<User> GetAsync(Guid id)
    {
        var user = await Db.Set<User>()
            .Where(x => x.Id == id)
            .FirstOrDefaultAsync();

        if (user is null)
            throw ApiException.NotFound($"{nameof(User)} with Id '{id}' is not found");

        return user;
    }

    public async Task<User?> GetByExternalIdAsync(Guid id)
    {
        return await Db.Set<User>()
            .Where(x => x.ExternalId == id)
            .FirstOrDefaultAsync();
    }

    public async Task<User> AddAsync(User user)
    {
        await Db.ThrowIfExistsByExternalIdAsync(typeof(User), user.ExternalId!.Value);
        Db.Add(user);

        return user;
    }
}