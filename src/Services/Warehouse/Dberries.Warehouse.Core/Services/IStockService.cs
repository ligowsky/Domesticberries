using BitzArt.Pagination;

namespace Dberries.Warehouse;

public interface IStockService
{
    public Task<PageResult<Stock>> GetStockPageAsync(Guid locationId, PageRequest pageRequest);
    public Task<Stock?> GetStockForItemAsync(Guid locationId, Guid itemId);
    public Task<Stock> UpdateStockForItemAsync(Guid locationId, Guid itemId, Stock stock);
}