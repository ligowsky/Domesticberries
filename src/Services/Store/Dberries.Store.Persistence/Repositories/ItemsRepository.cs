using BitzArt;
using BitzArt.Pagination;
using Microsoft.EntityFrameworkCore;

namespace Dberries.Store;

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
        var item = await Db.Set<Item>()
            .Where(x => x.ExternalId! == id)
            .FirstOrDefaultAsync();

        if (item is null)
            throw ApiException.NotFound($"{nameof(Item)} with id '{id}' is not found");

        return item;
    }

    public async Task<Item> Add(Item item)
    {
        await CheckExistsByExternalIdAsync<Item>(item.ExternalId!.Value, true);
        
        Db.Set<Item>().Add(item);

        return item;
    }

    public void Remove(Item item)
    {
        Db.Set<Item>().Remove(item);
    }
}