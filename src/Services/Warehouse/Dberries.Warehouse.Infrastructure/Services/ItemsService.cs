using BitzArt.Pagination;

namespace Dberries.Warehouse.Infrastructure;

public class ItemsService : IItemsService
{
    private readonly IItemsRepository _itemsRepository;

    public ItemsService(IItemsRepository itemsRepository)
    {
        _itemsRepository = itemsRepository;
    }

    public async Task<PageResult<Item>> GetItemsAsync(PageRequest pageRequest)
    {
        return await _itemsRepository.GetPageAsync(pageRequest);
    }

    public async Task<Item> GetItemAsync(Guid id)
    {
        return await _itemsRepository.GetAsync(id);
    }

    public async Task<Item> CreateItemAsync(Item item)
    {
        var createdItem = await _itemsRepository.CreateAsync(item);
        await _itemsRepository.SaveChangesAsync();

        return createdItem;
    }

    public async Task<Item> UpdateItemAsync(Guid id, Item item)
    {
        var existingItem = await _itemsRepository.GetAsync(id);
        existingItem.Update(item);
        await _itemsRepository.SaveChangesAsync();

        return existingItem;
    }

    public async Task DeleteItemAsync(Guid id)
    {
        var existingItem = await _itemsRepository.GetAsync(id);
        _itemsRepository.Delete(existingItem);
        await _itemsRepository.SaveChangesAsync();
    }
}