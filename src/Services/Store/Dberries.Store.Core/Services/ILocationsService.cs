namespace Dberries.Store;

public interface ILocationsService
{
    public Task<Location> AddAsync(Location location);
    public Task<Location> UpdateAsync(Guid id, Location location);
    public Task RemoveAsync(Guid id);
    public Task UpdateStockAsync(Guid locationId, Guid itemId, int quantity);
    public Task RemoveStockAsync(Guid locationId, Guid itemId);
}