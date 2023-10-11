using BitzArt.Pagination;

namespace Dberries.Warehouse;

public interface ILocationsService
{
    public Task<PageResult<Location>> GetPageAsync(PageRequest pageRequest);
    public Task<Location> GetAsync(Guid id);
    public Task<Location> AddAsync(Location location);
    public Task<Location> UpdateAsync(Guid id, Location location);
    public Task RemoveAsync(Guid id);
    
    Task<PageResult<Stock>> GetStockPageAsync(Guid locationId, PageRequest pageRequest);
    Task<Stock?> GetStockAsync(Guid locationId, Guid itemId);
    Task<Stock?> UpdateStockAsync(Guid locationId, Guid itemId, Stock stock);
}