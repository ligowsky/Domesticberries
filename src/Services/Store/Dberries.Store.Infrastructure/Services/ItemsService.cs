using BitzArt;
using BitzArt.Pagination;
using Nest;

namespace Dberries.Store.Infrastructure;

public class ItemsService : IItemsService
{
    private readonly IItemsRepository _itemsRepository;
    private readonly IElasticClient _elasticClient;

    public ItemsService(IItemsRepository itemsRepository, IElasticClient elasticClient)
    {
        _itemsRepository = itemsRepository;
        _elasticClient = elasticClient;
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

        await _elasticClient.IndexAsync(item, x => x.Index("items"));

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

        await _elasticClient.IndexAsync(existingItem, x => x.Index("items"));

        return existingItem;
    }

    public async Task RemoveAsync(Guid id)
    {
        var existingItem = await _itemsRepository.GetByExternalIdAsync(id);

        if (existingItem is null) return;

        _itemsRepository.Remove(existingItem);

        await _itemsRepository.SaveChangesAsync();

        await _elasticClient.DeleteAsync<Item>(id, x => x.Index("items"));
    }

    public async Task<PageResult<Item>> SearchAsync(PageRequest pageRequest, ISearchRequest searchRequest)
    {
        var searchResponse = await _elasticClient.SearchAsync<Item>(x => x
            .From(pageRequest.Offset!.Value)
            .Size(pageRequest.Limit!.Value)
            .Query(q => q
                .MultiMatch(m => m
                    .Query(searchRequest.Q)
                    .Fields(fs => fs
                        .Field(f => f.Name)
                        .Field(f => f.Description)
                    )
                )
            )
            .Source(s => s
                .Includes(i => i
                    .Field(f => f.Id)
                )
            )
        );

        var itemIds = searchResponse.Documents.Select(x => x.Id!.Value);

        return await _itemsRepository.GetByIdsAsync(pageRequest, itemIds);
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