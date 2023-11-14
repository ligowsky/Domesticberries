using BitzArt;

namespace Dberries.Store;

public interface ILocationsRepository : IRepository
{
    public Task<Location?> GetAsync(IFilterSet<Location> filter);
    public Task AddAsync(Location location);
    public void Remove(Location location);
    public Task UpdateStockAsync(Guid id, Stock stock);
    public Task RemoveStockAsync(Guid locationId, Guid itemId);
}