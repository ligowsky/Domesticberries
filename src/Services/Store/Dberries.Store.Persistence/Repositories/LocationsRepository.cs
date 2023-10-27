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

    public Task<Stock?> GetStockAsync(Guid locationId, Guid itemId)
    {
        throw new NotImplementedException();
    }

    public Task<Stock?> UpdateStockAsync(Guid locationId, Guid itemId, int quantity)
    {
        throw new NotImplementedException();
    }
}