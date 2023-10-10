using BitzArt.Pagination;

namespace Dberries.Warehouse;

public interface IItemsService
{
    public Task<PageResult<Item>> GetPageAsync(PageRequest pageRequest);
    public Task<Item> GetAsync(Guid id);
    public Task<Item> AddAsync(Item item);
    public Task<Item> UpdateAsync(Guid id, Item item);
    public Task RemoveAsync(Guid id);
}