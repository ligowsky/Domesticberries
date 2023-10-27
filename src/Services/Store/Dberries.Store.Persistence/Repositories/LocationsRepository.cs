using BitzArt;
using Microsoft.EntityFrameworkCore;

namespace Dberries.Store.Persistence;

public class LocationsRepository : RepositoryBase, ILocationsRepository
{
    public LocationsRepository(AppDbContext db) : base(db)
    {
    }

    public async Task<Location> GetAsync(Guid id)
    {
        var location = await Db.Set<Location>()
            .Where(x => x.ExternalId! == id)
            .FirstOrDefaultAsync();

        if (location is null)
            throw ApiException.NotFound($"{nameof(Location)} with id '{id}' is not found");

        return location;
    }

    public async Task<Location> Add(Location location)
    {
        await CheckExistsByExternalIdAsync<Location>(location.ExternalId!.Value, true);

        Db.Set<Location>().Add(location);

        return location;
    }

    public void Remove(Location location)
    {
        Db.Set<Location>().Remove(location);
    }

    public async Task<Stock?> GetStockAsync(Guid locationId, Guid itemId)
    {
        await CheckExistsByExternalIdAsync<Location>(locationId, true);
        await CheckExistsByExternalIdAsync<Item>(itemId, true);

        return await Db.Set<Location>()
            .AsNoTracking()
            .Where(x => x.ExternalId == locationId)
            .SelectMany(x => x.Stock!
                .Where(y => y.ItemId == itemId))
            .Include(x => x.Item)
            .OrderBy(x => x.ItemId)
            .FirstOrDefaultAsync();
    }

    public async Task<Stock?> UpdateStockAsync(Guid locationId, Guid itemId, int quantity)
    {
        if (quantity < 0)
            throw ApiException.BadRequest($"Invalid quantity: {quantity}. Quantity must be greater than 0.");

        var item = await Db.Set<Item>()
            .Where(x => x.ExternalId == itemId)
            .FirstOrDefaultAsync();

        if (item is null)
            throw ApiException.NotFound($"{nameof(Item)} with ExternalId '{itemId}' is not found");

        var location = await Db.Set<Location>()
            .Where(x => x.ExternalId == locationId)
            .Include(x => x.Stock!
                .Where(y => y.ItemId == item.Id))
            .FirstOrDefaultAsync();

        if (location is null)
            throw ApiException.NotFound($"{nameof(Location)} with ExternalId '{locationId}' is not found");
        

        var stock = location.Stock!.FirstOrDefault(x => x.ItemId == item.Id);

        if (stock is null)
        {
            stock = new Stock
            {
                ItemId = item.Id,
            };

            location.Stock!.Add(stock);
        }

        stock.Quantity = quantity;

        if (quantity == 0)
            location.Stock!.Remove(stock);

        return stock;
    }
}