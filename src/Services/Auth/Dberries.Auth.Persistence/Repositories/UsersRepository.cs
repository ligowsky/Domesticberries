using BitzArt;
using Microsoft.EntityFrameworkCore;

namespace Dberries.Auth.Persistence;

public class UsersRepository : RepositoryBase, IUsersRepository
{
    public UsersRepository(AppDbContext db) : base(db)
    {
    }

    public async Task<User> GetAsync(Guid id)
    {
        var result = await Db.Set<User>()
            .Where(x => x.Id! == id)
            .FirstOrDefaultAsync();

        if (result is null)
            throw ApiException.NotFound($"{nameof(User)} with Id '{id}' is not found");

        return result;
    }

    public async Task<User> GetByEmailAsync(string email)
    {
        var result = await Db.Set<User>()
            .Where(x => x.Email! == email)
            .FirstOrDefaultAsync();

        if (result is null)
            throw ApiException.NotFound($"{nameof(User)} with Email '{email}' is not found");

        return result;
    }

    public async Task<bool> ThrowIfExistsByEmailAsync(string email)
    {
        var exists = await Db.Set<User>()
            .Where(x => x.Email! == email)
            .AnyAsync();

        if (exists)
            throw ApiException.BadRequest($"{nameof(User)} with Email '{email}' already exists");

        return exists;
    }

    public User Add(User user)
    {
        Db.Add(user);

        return user;
    }
}