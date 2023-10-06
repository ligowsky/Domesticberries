using BitzArt.Pagination;

namespace Dberries.Warehouse;

public interface IStockRepository : IRepositoryBase
{
    public Task<PageResult<Stock>> GetPageAsync(Guid locationId, PageRequest pageRequest);
    public Task<Stock> GetAsync(Guid locationId, Guid itemId);
}