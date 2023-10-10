using BitzArt.Pagination;

namespace Dberries.Warehouse;

public interface ILocationsRepository : IRepositoryBase
{
    public Task<PageResult<Location>> GetPageAsync(PageRequest pageRequest);
    public Task<Location> GetAsync(Guid id);
    public Location Add(Location location);
    public void Delete(Location location);
    
    public Task<PageResult<Stock>> GetStockPageAsync(Guid locationId, PageRequest pageRequest);
    public Task<Stock?> GetStockAsync(Guid locationId, Guid itemId);
    public Task<Stock?> UpdateStockAsync(Guid locationId, Guid itemId, int quantity);
}