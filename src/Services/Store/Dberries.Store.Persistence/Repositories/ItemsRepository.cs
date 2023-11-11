using BitzArt;
using BitzArt.Pagination;
using Microsoft.EntityFrameworkCore;
using Nest;

namespace Dberries.Store.Persistence;

public class ItemsRepository : RepositoryBase, IItemsRepository
{
    private readonly ElasticClient _elasticClient;

    public ItemsRepository(AppDbContext db, ElasticClient elasticClient) : base(db)
    {
        _elasticClient = elasticClient;
    }

    public async Task<PageResult<Item>> GetPageAsync(PageRequest pageRequest)
    {
        return await Db.Set<Item>()
            .OrderBy(x => x.Id)
            .ToPageAsync(pageRequest);
    }

    public async Task<Item> GetAsync(Guid id)
    {
        var item = await Db.Set<Item>()
            .Where(x => x.Id == id)
            .FirstOrDefaultAsync();

        if (item is null)
            throw ApiException.NotFound($"{nameof(Item)} with Id '{id}' is not found");

        return item;
    }

    public async Task<Item?> GetByExternalIdAsync(Guid id)
    {
        return await Db.Set<Item>()
            .Where(x => x.ExternalId == id)
            .FirstOrDefaultAsync();
    }

    public async Task<PageResult<Item>> SearchAsync(PageRequest pageRequest, SearchRequestDto searchRequest)
    {
        var searchResponse = await _elasticClient.SearchAsync<Item>(x => x
            .From(pageRequest.Offset!.Value)
            .Size(pageRequest.Limit!.Value)
            .Query(q => q
                .MultiMatch(m => m
                    .Query(searchRequest.Q!)
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

        return await Db.Set<Item>()
            .Where(item => itemIds.Contains(item.Id!.Value))
            .OrderBy(x => x.Id)
            .ToPageAsync(pageRequest);
    }

    public async Task<Item> AddAsync(Item item)
    {
        await Db.ThrowIfExistsByExternalIdAsync(typeof(Item), item.ExternalId!.Value);
        Db.Add(item);

        await _elasticClient.IndexDocumentAsync(item);

        return item;
    }

    public async Task<Item> UpdateAsync(Item item)
    {
        var existingItem = await GetByExternalIdAsync(item.ExternalId!.Value);

        if (existingItem is null)
        {
            existingItem = await AddAsync(item);
        }
        else
        {
            existingItem.Patch(item)
                .Property(x => x.Name)
                .Property(x => x.Description);
        }

        await _elasticClient.IndexDocumentAsync(existingItem);

        return existingItem;
    }

    public async Task RemoveAsync(Item item)
    {
        Db.Remove(item);

        await _elasticClient.DeleteAsync<Item>(item.Id!);
    }

    public async Task<ItemAvailabilityResponse> GetAvailabilityAsync(Guid id)
    {
        await Db.ThrowIfNotExistsAsync<Item>(id);

        var availableInLocations = await Db.Set<Location>()
            .Where(x => x.Stock!
                .Any(y => y.ItemId == id))
            .OrderBy(x => x.Id)
            .Select(x => new ItemAvailabilityInLocation
            {
                LocationId = x.Id,
                LocationName = x.Name,
                Quantity = x.Stock!.First(y => y.ItemId == id).Quantity
            })
            .ToListAsync();

        return new ItemAvailabilityResponse(availableInLocations);
    }

    public async Task<Item> UpdateRatingAsync(Guid itemId, Rating input)
    {
        if (input.Value < Rating.MinValue || input.Value > Rating.MaxValue)
            throw ApiException.BadRequest(
                $"{nameof(Rating)} value must be between {Rating.MinValue} and {Rating.MaxValue}");

        var item = await Db.Set<Item>()
            .Where(x => x.Id == itemId)
            .Include(x => x.Ratings)
            .FirstOrDefaultAsync();

        if (item is null)
            throw ApiException.NotFound($"{nameof(Item)} with Id '{itemId}' is not found");

        var rating = item.Ratings!.FirstOrDefault(x => x.UserId == input.UserId);

        if (rating is null)
        {
            rating = input;
            item.Ratings!.Add(rating);
        }

        if (rating.Value == 0)
            item.Ratings!.Remove(rating);

        return item;
    }
}