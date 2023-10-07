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
            throw ApiException.NotFound($"{nameof(Location)} with id '{id}' is not found");

        return result;
    }

    public async Task<Location> CreateAsync(Location location)
    {
        await Db.Set<Location>().AddAsync(location);

        return location;
    }

    public void Delete(Location location)
    {
        Db.Set<Location>().Remove(location);
    }

    public async Task<PageResult<Stock>> GetStockPageAsync(Guid locationId, PageRequest pageRequest)
    {
        await CheckExistsAsync<Location>(locationId);

        return await Db.Set<Location>()
            .AsNoTracking()
            .Where(x => x.Id == locationId)
            .SelectMany(x => x.Stock!)
            .Include(x => x.Item)
            .ToPageAsync(pageRequest);
    }

    public async Task<Stock?> GetStockAsync(Guid locationId, Guid itemId)
    {
        await CheckExistsAsync<Location>(locationId);
        await CheckExistsAsync<Item>(itemId);

        var location = await Db.Set<Location>()
            .Where(x => x.Id == locationId)
            .Include(x => x.Stock!.Where(y => y.ItemId == itemId))
            .ThenInclude(stock => stock.Item)
            .FirstOrDefaultAsync();

        return location?.Stock!.FirstOrDefault();
    }

    public async Task<Stock?> UpdateStockAsync(Guid locationId, Guid itemId, int quantity)
    {
        await CheckExistsAsync<Item>(itemId);

        var location = await Db.Set<Location>()
            .Where(x => x.Id == locationId)
            .Include(x => x.Stock!.Where(y => y.ItemId == itemId))
            .FirstOrDefaultAsync();

        if (location is null)
            throw ApiException.NotFound($"{nameof(Location)} with id '{locationId}' is not found");

        return location.UpdateStock(itemId, quantity);
    }
}