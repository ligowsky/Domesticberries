using BitzArt.Pagination;

namespace Dberries.Warehouse;

public interface ILocationsService
{
    public Task<PageResult<Location>> GetLocationsPageAsync(PageRequest pageRequest);
    public Task<Location> GetLocationAsync(Guid id);
    public Task<Location> CreateLocationAsync(Location location);
    public Task<Location> UpdateLocationAsync(Guid id, Location location);
    public Task DeleteLocationAsync(Guid id);
    
    Task<PageResult<Stock>> GetStockPageAsync(Guid locationId, PageRequest pageRequest);
    Task<Stock?> GetStockAsync(Guid locationId, Guid itemId);
    Task<Stock?> UpdateStockAsync(Guid locationId, Guid itemId, int quantity);
}