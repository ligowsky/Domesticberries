using BitzArt;

namespace Dberries.Store;

public interface ILocationsRepository : IRepository
{
    public Task<Location?> GetAsync(IFilterSet<Location> filter);
    public void Add(Location location);
    public void Update(Location existingLocation, Location location);
    public void Remove(Location location);
    public Task UpdateStockAsync(IFilterSet<Location> filter, Stock stock);
    public Task RemoveStockAsync(IFilterSet<Location> filter, Guid itemId);
}