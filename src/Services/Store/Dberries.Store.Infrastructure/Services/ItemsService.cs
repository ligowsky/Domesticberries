using BitzArt;
using BitzArt.Pagination;

namespace Dberries.Store.Infrastructure;

public class ItemsService : IItemsService
{
    private readonly IItemsRepository _itemsRepository;
    private readonly IUsersService _usersService;

    public ItemsService(IItemsRepository itemsRepository, IUsersService usersService)
    {
        _itemsRepository = itemsRepository;
        _usersService = usersService;
    }

    public async Task<PageResult<Item>> GetPageAsync(PageRequest pageRequest)
    {
        return await _itemsRepository.GetPageAsync(pageRequest);
    }

    public async Task<Item> GetAsync(IFilterSet<Item> filter)
    {
        var item = await _itemsRepository.GetAsync(filter);

        if (item is null)
            throw ApiException.NotFound($"{nameof(Item)} is not found");

        return item;
    }

    public async Task<Item> AddAsync(Item item)
    {
        await _itemsRepository.AddAsync(item);
        await _itemsRepository.SaveChangesAsync();

        return item;
    }

    public async Task<Item> UpdateAsync(Guid id, Item item)
    {
        var filter = new ItemFilterSet { ExternalId = id };
        var existingItem = await _itemsRepository.GetAsync(filter);

        if (existingItem is null)
        {
            await _itemsRepository.AddAsync(item);
            await _itemsRepository.SaveChangesAsync();
            return item;
        }

        await _itemsRepository.UpdateAsync(existingItem, item);
        await _itemsRepository.SaveChangesAsync();

        return existingItem;
    }

    public async Task RemoveAsync(Guid id)
    {
        var filter = new ItemFilterSet { ExternalId = id };
        var existingItem = await _itemsRepository.GetAsync(filter);

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

    public async Task<Item> UpdateRatingAsync(Guid itemId, Guid userId, byte value)
    {
        var userFilter = new UserFilterSet { ExternalId = userId };
        var user = await _usersService.GetAsync(userFilter);

        var rating = new Rating(user.Id!.Value, value);

        await _itemsRepository.UpdateRatingAsync(itemId, rating);
        await _itemsRepository.SaveChangesAsync();

        var itemFilter = new ItemFilterSet { Id = itemId };

        return await GetAsync(itemFilter);
    }

    public async Task<Item> RemoveRatingAsync(Guid itemId, Guid userId)
    {
        var userFilter = new UserFilterSet { ExternalId = userId };
        var user = await _usersService.GetAsync(userFilter);

        await _itemsRepository.RemoveRatingAsync(itemId, user.Id!.Value);
        await _itemsRepository.SaveChangesAsync();

        var itemFilter = new ItemFilterSet { Id = itemId };

        return await GetAsync(itemFilter);
    }
}