using BitzArt;
using Microsoft.EntityFrameworkCore;

namespace Dberries.Store.Persistence;

public class LocationsRepository : RepositoryBase, ILocationsRepository
{
    public LocationsRepository(AppDbContext db) : base(db)
    {
    }

    public async Task<Location?> GetAsync(IFilterSet<Location> filterSet)
    {
        return await Db.Set<Location>()
            .Apply(filterSet)
            .FirstOrDefaultAsync();
    }

    public void Add(Location location)
    {
        Db.Set<Location>().Add(location);
    }

    public void Update(Location existingLocation, Location location)
    {
        existingLocation.Patch(location)
            .Property(x => x.Name);
    }

    public void Remove(Location location)
    {
        Db.Set<Location>().Remove(location);
    }

    public async Task UpdateStockAsync(IFilterSet<Location> filterSet, Stock input)
    {
        var location = await GetWithItemStock(filterSet, input.ItemId!.Value);
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

    public async Task RemoveStockAsync(IFilterSet<Location> filterSet, Guid itemId)
    {
        var location = await GetWithItemStock(filterSet, itemId);
        var stock = location.Stock!.FirstOrDefault(x => x.ItemId == itemId);

        if (stock is not null)
            location.Stock!.Remove(stock);
    }

    private async Task<Location> GetWithItemStock(IFilterSet<Location> filterSet, Guid itemId)
    {
        var location = await Db.Set<Location>()
            .Apply(filterSet)
            .Include(x => x.Stock!
                .Where(y => y.ItemId == itemId))
            .FirstOrDefaultAsync();

        if (location is null)
            throw ApiException.NotFound($"{nameof(Location)} is not found");

        return location;
    }
}