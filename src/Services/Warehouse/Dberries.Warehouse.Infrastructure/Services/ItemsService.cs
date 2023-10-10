using BitzArt;
using BitzArt.Pagination;

namespace Dberries.Warehouse.Infrastructure;

public class ItemsService : IItemsService
{
    private readonly IItemsRepository _itemsRepository;

    public ItemsService(IItemsRepository itemsRepository)
    {
        _itemsRepository = itemsRepository;
    }

    public async Task<PageResult<Item>> GetPageAsync(PageRequest pageRequest)
    {
        return await _itemsRepository.GetPageAsync(pageRequest);
    }

    public async Task<Item> GetAsync(Guid id)
    {
        return await _itemsRepository.GetAsync(id);
    }

    public async Task<Item> AddAsync(Item item)
    {
        var createdItem = _itemsRepository.Add(item);
        await _itemsRepository.SaveChangesAsync();

        return createdItem;
    }

    public async Task<Item> UpdateAsync(Guid id, Item item)
    {
        var existingItem = await _itemsRepository.GetAsync(id);

        existingItem.Patch(item)
            .Property(x => x.Name)
            .Property(x => x.Description);
        
        await _itemsRepository.SaveChangesAsync();

        return existingItem;
    }

    public async Task RemoveAsync(Guid id)
    {
        var existingItem = await _itemsRepository.GetAsync(id);
        _itemsRepository.Remove(existingItem);
        await _itemsRepository.SaveChangesAsync();
    }
}