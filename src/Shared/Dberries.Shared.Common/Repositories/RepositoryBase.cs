using BitzArt;
using Microsoft.EntityFrameworkCore;

namespace Dberries;

public class RepositoryBase : IRepository
{
    protected readonly AppDbContext Db;

    protected RepositoryBase(AppDbContext db)
    {
        Db = db;
    }

    public async Task<int> SaveChangesAsync()
    {
        return await Db.SaveChangesAsync();
    }

    protected async Task CheckExistsAsync<T>(Guid id) where T : class, IEntity
    {
        var entityExists = await Db.Set<T>()
            .Where(x => x.Id == id)
            .AnyAsync();

        if (!entityExists)
            throw ApiException.NotFound($"{typeof(T).Name} with id '{id}' is not found");
    }
    
    protected async Task<bool> CheckExistsByExternalIdAsync<T>(Guid id) where T : class, IExternalId
    {
        return await Db.Set<T>()
            .Where(x => x.ExternalId == id)
            .AnyAsync();
    }
}