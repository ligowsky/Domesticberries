namespace Dberries.Store;

public interface ILocationsService
{
    public Task AddAsync(Location location);
    public Task UpdateAsync(Location location);
    public Task RemoveAsync(Guid id);
    public Task UpdateStockAsync(Guid locationId, Guid itemId, int quantity);
}