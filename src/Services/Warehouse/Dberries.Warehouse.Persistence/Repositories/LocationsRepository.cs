using BitzArt;
using BitzArt.Pagination;
using Microsoft.EntityFrameworkCore;

namespace Dberries.Warehouse.Persistence;

public class LocationsRepository : RepositoryBase, ILocationsRepository
{
    public LocationsRepository(AppDbContext db) : base(db)
    {
    }

    public async Task<PageResult<Location>> GetPageAsync(PageRequest pageRequest)
    {
        return await Db.Set<Location>().ToPageAsync(pageRequest);
    }

    public async Task<Location> GetAsync(Guid id)
    {
        var result = await Db.Set<Location>()
            .Where(x => x.Id! == id)
            .FirstOrDefaultAsync();

        if (result is null)
            throw ApiException.NotFound($"{nameof(Location)} with Id '{id}' is not found");

        return result;
    }

    public Location Add(Location location)
    {
        Db.Set<Location>().Add(location);

        return location;
    }

    public void Remove(Location location)
    {
        Db.Set<Location>().Remove(location);
    }

    public async Task<PageResult<Stock>> GetStockPageAsync(Guid locationId, PageRequest pageRequest)
    {
        await CheckExistsAsync<Location>(locationId, true);

        return await Db.Set<Location>()
            .AsNoTracking()
            .Where(x => x.Id == locationId)
            .SelectMany(x => x.Stock!)
            .Include(x => x.Item)
            .OrderBy(x => x.ItemId)
            .ToPageAsync(pageRequest);
    }

    public async Task<Stock?> GetStockAsync(Guid locationId, Guid itemId)
    {
        await CheckExistsAsync<Location>(locationId, true);
        await CheckExistsAsync<Item>(itemId, true);

        return await Db.Set<Location>()
            .AsNoTracking()
            .Where(x => x.Id == locationId)
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

        await CheckExistsAsync<Item>(itemId, true);

        var location = await Db.Set<Location>()
            .Where(x => x.Id == locationId)
            .Include(x => x.Stock!
                .Where(y => y.ItemId == itemId))
            .FirstOrDefaultAsync();

        if (location is null)
            throw ApiException.NotFound($"{nameof(Location)} with Id '{locationId}' is not found");

        var stock = location.Stock!.FirstOrDefault(x => x.ItemId == itemId);

        if (stock is null)
        {
            stock = new Stock
            {
                ItemId = itemId,
            };

            location.Stock!.Add(stock);
        }

        stock.Quantity = quantity;

        if (quantity == 0)
            location.Stock!.Remove(stock);

        return stock;
    }
}