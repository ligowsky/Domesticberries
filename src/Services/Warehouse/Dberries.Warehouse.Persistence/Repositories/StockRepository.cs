using BitzArt;
using BitzArt.Pagination;
using Microsoft.EntityFrameworkCore;

namespace Dberries.Warehouse.Persistence;

public class StockRepository : RepositoryBase, IStockRepository
{
    public StockRepository(AppDbContext db) : base(db)
    {
    }

    public async Task<PageResult<Stock>> GetPageAsync(Guid locationId, PageRequest pageRequest)
    {
        return await Db.Set<Location>()
            .Where(x => x.Id == locationId)
            .SelectMany(x => x.Stock!)
            .Include(x => x.Item)
            .ToPageAsync(pageRequest);
    }

    public async Task<Stock> GetAsync(Guid locationId, Guid itemId)
    {
        var stock = await Db.Set<Location>()
            .Where(x => x.Id == locationId)
            .SelectMany(x => x.Stock!)
            .Include(x => x.Item)
            .Where(x => x.Item!.Id == itemId)
            .FirstOrDefaultAsync();

        if (stock is null)
        {
            throw ApiException.NotFound(
                $"Stock for the item with id '{itemId}' in the location with id '{locationId}' is not found");
        }

        return stock;
    }
}