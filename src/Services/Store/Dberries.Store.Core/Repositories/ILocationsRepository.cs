using BitzArt.Pagination;

namespace Dberries.Store;

public interface ILocationsRepository : IRepository
{
    public Task<Location> GetAsync(Guid id);
    public Task<Location> Add(Location location);
    public void Remove(Location location);
    public Task<Stock?> UpdateStockAsync(Guid locationId, Guid itemId, int quantity);
}