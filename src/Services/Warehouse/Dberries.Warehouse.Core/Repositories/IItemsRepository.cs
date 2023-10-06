using BitzArt.Pagination;

namespace Dberries.Warehouse;

public interface IItemsRepository : IRepositoryBase
{
    public Task<PageResult<Item>> GetPageAsync(PageRequest pageRequest);
    public Task<Item> GetAsync(Guid id);
    public Task<Item> CreateAsync(Item item);
    public void Delete(Item item);
    public Task<bool> Exists(Guid id);
}