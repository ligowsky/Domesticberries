using BitzArt.Pagination;

namespace Dberries.Warehouse;

public interface IItemsRepository : IRepositoryBase
{
    public Task<PageResult<Item>> GetPageAsync(PageRequest pageRequest);
    public Task<Item> GetAsync(Guid id);
    public Item Add(Item item);
    public void Remove(Item item);
}