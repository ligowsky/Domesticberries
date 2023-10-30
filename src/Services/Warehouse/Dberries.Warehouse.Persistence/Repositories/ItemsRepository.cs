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
            throw ApiException.NotFound($"{nameof(Item)} with Id '{id}' is not found");

        return result;
    }

    public Item Add(Item item)
    {
        Db.Set<Item>().Add(item);

        return item;
    }

    public void Remove(Item item)
    {
        Db.Set<Item>().Remove(item);
    }
}