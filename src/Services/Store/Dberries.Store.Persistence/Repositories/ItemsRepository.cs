using BitzArt;
using BitzArt.Pagination;
using Microsoft.EntityFrameworkCore;

namespace Dberries.Store.Persistence;

public class ItemsRepository : RepositoryBase, IItemsRepository
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

    public async Task<Item?> GetByExternalIdAsync(Guid id)
    {
        return await Db.Set<Item>()
            .Where(x => x.ExternalId == id)
            .FirstOrDefaultAsync();
    }

    public async Task<PageResult<Item>> GetByIdsAsync(PageRequest pageRequest, IEnumerable<Guid> ids)
    {
        return await Db.Set<Item>()
            .Where(item => ids.Contains(item.Id!.Value))
            .OrderBy(x => x.Id)
            .ToPageAsync(pageRequest);
    }

    public async Task<Item> AddAsync(Item item)
    {
        await Db.ThrowIfExistsByExternalIdAsync(typeof(Item), item.ExternalId!.Value);
        Db.Add(item);

        return item;
    }

    public void Remove(Item item)
    {
        Db.Remove(item);
    }

    public async Task<ItemAvailabilityResponse> GetAvailabilityAsync(Guid id)
    {
        await Db.ThrowIfNotExistsAsync<Item>(id);

        var availableInLocations = await Db.Set<Location>()
            .Where(x => x.Stock!
                .Any(y => y.ItemId == id))
            .OrderBy(x => x.Id)
            .Select(x => new ItemAvailabilityInLocation
            {
                LocationId = x.Id,
                LocationName = x.Name,
                Quantity = x.Stock!.First(y => y.ItemId == id).Quantity
            })
            .ToListAsync();

        return new ItemAvailabilityResponse(availableInLocations);
    }
}