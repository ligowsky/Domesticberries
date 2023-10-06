using BitzArt.Pagination;

namespace Dberries.Warehouse;

public interface IItemsService
{
    public Task<PageResult<Item>> GetItemsAsync(PageRequest pageRequest);
    public Task<Item> GetItemAsync(Guid id);
    public Task<Item> CreateItemAsync(Item item);
    public Task<Item> UpdateItemAsync(Guid id, Item item);
    public Task DeleteItemAsync(Guid id);
}