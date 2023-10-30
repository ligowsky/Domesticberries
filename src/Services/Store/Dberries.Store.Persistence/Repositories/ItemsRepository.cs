using BitzArt;
using BitzArt.Pagination;
using Microsoft.EntityFrameworkCore;

namespace Dberries.Store.Persistence;

public class ItemsRepository : EntityRepository, IItemsRepository
{
    public ItemsRepository(AppDbContext db) : base(db)
    {
    }

    public async Task<PageResult<Item>> GetPageAsync(PageRequest pageRequest)
    {
        return await Db.Set<Item>()
            .OrderBy(x => x.Id)
            .ToPageAsync(pageRequest);
    }

    public async Task<Item> GetAsync(Guid id)
    {
        var item = await Db.Set<Item>()
            .Where(x => x.Id == id)
            .FirstOrDefaultAsync();

        if (item is null)
            throw ApiException.NotFound($"{nameof(Item)} with Id '{id}' is not found");

        return item;
    }

    public async Task<Item?> GetByExternalIdAsync(Guid id, bool throwException = true)
    {
        var item = await Db.Set<Item>()
            .Where(x => x.ExternalId == id)
            .FirstOrDefaultAsync();

        if (item is null & throwException)
            throw ApiException.NotFound($"{nameof(Item)} with External '{id}' is not found");

        return item;
    }

    public Item AddAsync(Item item)
    {
        Db.Set<Item>().Add(item);

        return item;
    }

    public void Remove(Item item)
    {
        Db.Set<Item>().Remove(item);
    }

    public async Task<PageResult<Location>> GetAvailabilityAsync(PageRequest pageRequest, Guid id)
    {
        await CheckExistsAsync<Item>(id, true);

        return await Db.Set<Location>()
            .Where(x => x.Stock!
                .Any(y => y.ItemId == id))
            .OrderBy(x => x.Id)
            .ToPageAsync(pageRequest);
    }
}