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
            throw ApiException.NotFound($"Location with id '{id}' is not found");

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

    public async Task<bool> Exists(Guid id)
    {
        var locationExists = await Db.Set<Location>()
            .Where(x => x.Id == id)
            .AnyAsync();

        if (!locationExists)
            throw ApiException.NotFound($"Location with id '{id}' is not found");

        return locationExists;
    }
}