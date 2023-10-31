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

    public async Task<ICollection<ItemAvailability>> GetAvailabilityAsync(Guid id)
    {
        await Db.ThrowIfNotExistsAsync<Item>(id);

        var locations = await Db.Set<Location>()
            .Where(x => x.Stock!
                .Any(y => y.ItemId == id))
            .Include(x => x.Stock!.Where(y => y.ItemId == id))
            .OrderBy(x => x.Id)
            .ToListAsync();

        var itemAvailabilityList = new List<ItemAvailability>();

        foreach (var location in locations)
        {
            var quantity = location.Stock!.First(x => x.ItemId == id).Quantity;

            var itemAvailability = new ItemAvailability
            {
                Location = location,
                Quantity = quantity
            };

            itemAvailabilityList.Add(itemAvailability);
        }

        return itemAvailabilityList;
    }
}