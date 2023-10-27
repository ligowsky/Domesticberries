using BitzArt.Pagination;

namespace Dberries.Store;

public interface IItemsRepository : IRepository
{
    public Task<PageResult<Item>> GetPageAsync(PageRequest pageRequest);
    public Task<Item> GetAsync(Guid id);
    public Task<Item> Add(Item item);
    public void Remove(Item item);
}