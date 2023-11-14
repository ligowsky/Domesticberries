using BitzArt;
using Microsoft.EntityFrameworkCore;

namespace Dberries.Store.Persistence;

public class LocationsRepository : RepositoryBase, ILocationsRepository
{
    public LocationsRepository(AppDbContext db) : base(db)
    {
    }

    public async Task<Location?> GetAsync(IFilterSet<Location> filter)
    {
        return await Db.Set<Location>()
            .Apply(filter)
            .FirstOrDefaultAsync();
    }

    public async Task AddAsync(Location location)
    {
        await Db.ThrowIfExistsByExternalIdAsync(typeof(Location), location.ExternalId!.Value);
        Db.Set<Location>().Add(location);
    }

    public void Remove(Location location)
    {
        Db.Set<Location>().Remove(location);
    }

    public async Task UpdateStockAsync(Guid locationId, Stock input)
    {
        var location = await GetWithItemStock(locationId, input.ItemId!.Value);
        var stock = location.Stock!.FirstOrDefault(x => x.ItemId == input.ItemId);

        if (stock is null)
        {
            location.Stock!.Add(input);
        }
        else
        {
            stock.Quantity = input.Quantity;
        }
    }

    public async Task RemoveStockAsync(Guid locationId, Guid itemId)
    {
        var location = await GetWithItemStock(locationId, itemId);
        var stock = location.Stock!.FirstOrDefault(x => x.ItemId == itemId);

        if (stock is not null)
            location.Stock!.Remove(stock);
    }

    private async Task<Location> GetWithItemStock(Guid locationId, Guid itemId)
    {
        var location = await Db.Set<Location>()
            .Where(x => x.ExternalId == locationId)
            .Include(x => x.Stock!
                .Where(y => y.ItemId == itemId))
            .FirstOrDefaultAsync();

        if (location is null)
            throw ApiException.NotFound($"{nameof(Location)} is not found");

        return location;
    }
}