using BitzArt.ApiExceptions;
using Microsoft.EntityFrameworkCore;

namespace Dberries.Warehouse.Persistence;

public class ItemsRepository : RepositoryBase, IItemsRepository
{
    protected ItemsRepository(AppDbContext db) : base(db)
    {
    }

    public async Task<List<Item>> GetAllAsync()
    {
        return await Db.Set<Item>().ToListAsync();
    }

    public async Task<Item> GetAsync(Guid? id)
    {
        var result = await Db.Set<Item>()
            .Where(x => x.Id == id)
            .FirstOrDefaultAsync();

        if (result is null) throw new NotFoundApiException($"Item with id '{id}' is not found");

        return result;
    }

    public async Task<Item> CreateAsync(Item item)
    {
        await Db.Set<Item>().AddAsync(item);

        return item;
    }

    public void Delete(Item item)
    {
        Db.Set<Item>().Remove(item);
    }
}