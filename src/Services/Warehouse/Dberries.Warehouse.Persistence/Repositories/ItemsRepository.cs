using BitzArt;
using BitzArt.Pagination;
using Microsoft.EntityFrameworkCore;

namespace Dberries.Warehouse.Persistence;

public class ItemsRepository : RepositoryBase, IItemsRepository
{
    public ItemsRepository(AppDbContext db) : base(db)
    {
    }

    public async Task<PageResult<Item>> GetPageAsync(PageRequest pageRequest)
    {
        return await Db.Set<Item>().ToPageAsync(pageRequest);
    }

    public async Task<Item> GetAsync(Guid id)
    {
        var result = await Db.Set<Item>()
            .Where(x => x.Id! == id)
            .FirstOrDefaultAsync();

        if (result is null)
            throw ApiException.NotFound($"Item with id '{id}' is not found");

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

    public async Task<bool> Exists(Guid id)
    {
        var itemExists = await Db.Set<Item>()
            .Where(x => x.Id == id)
            .AnyAsync();

        if (!itemExists)
            throw ApiException.NotFound($"Item with id '{id}' is not found");

        return itemExists;
    }
}