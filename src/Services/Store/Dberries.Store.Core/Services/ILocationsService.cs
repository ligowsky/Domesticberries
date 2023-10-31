namespace Dberries.Store;

public interface ILocationsService
{
    public Task<Location> AddAsync(Location location);
    public Task<Location> UpdateAsync(Location location);
    public Task RemoveAsync(Guid id);
    public Task<Stock?> UpdateStockAsync(Guid locationId, Guid itemId, int quantity);
}