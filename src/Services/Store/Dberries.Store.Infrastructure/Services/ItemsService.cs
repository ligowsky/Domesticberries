using BitzArt;
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
        var existingItem = await _itemsRepository.GetByExternalIdAsync(item.ExternalId!.Value);

        if (existingItem is null)
        {
            existingItem = await _itemsRepository.AddAsync(item);
        }
        else
        {
            existingItem.Patch(item)
                .Property(x => x.Name)
                .Property(x => x.Description);
        }

        await _itemsRepository.SaveChangesAsync();

        return existingItem;
    }

    public async Task RemoveAsync(Guid id)
    {
        var existingItem = await _itemsRepository.GetByExternalIdAsync(id);

        if (existingItem is null) return;

        _itemsRepository.Remove(existingItem);
        await _itemsRepository.SaveChangesAsync();
    }

    public Task<PageResult<Item>> SearchAsync(PageRequest pageRequest, string query)
    {
        throw new NotImplementedException();
    }

    public Task<ItemAvailabilityResponse> GetAvailabilityAsync(Guid id)
    {
        return _itemsRepository.GetAvailabilityAsync(id);
    }

    public Task UpdateRatingAsync(Guid id, byte value)
    {
        throw new NotImplementedException();
    }
}