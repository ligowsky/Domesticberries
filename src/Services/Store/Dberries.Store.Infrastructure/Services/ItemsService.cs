using BitzArt.Pagination;

namespace Dberries.Store.Infrastructure;

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
        await _itemsRepository.AddAsync(item);
        await _itemsRepository.SaveChangesAsync();
        
        return item;
    }

    public async Task<Item> UpdateAsync(Item item)
    {
        var updatedItem = await _itemsRepository.UpdateAsync(item);
        await _itemsRepository.SaveChangesAsync();

        return updatedItem;
    }

    public async Task RemoveAsync(Guid id)
    {
        var existingItem = await _itemsRepository.GetByExternalIdAsync(id);

        if (existingItem is null) return;
        
        await _itemsRepository.RemoveAsync(existingItem);

        await _itemsRepository.SaveChangesAsync();
    }

    public async Task<PageResult<Item>> SearchAsync(PageRequest pageRequest, SearchRequestDto searchRequest)
    {
        return await _itemsRepository.SearchAsync(pageRequest, searchRequest);
    }

    public Task<ItemAvailabilityResponse> GetAvailabilityAsync(Guid id)
    {
        return _itemsRepository.GetAvailabilityAsync(id);
    }

    public Task<Item> UpdateRatingAsync(Guid itemId, Guid userId, byte value)
    {
        throw new NotImplementedException();
    }
}