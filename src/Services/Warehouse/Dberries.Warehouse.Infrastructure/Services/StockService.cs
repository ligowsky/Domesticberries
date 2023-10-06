using BitzArt.Pagination;

namespace Dberries.Warehouse.Infrastructure;

public class StockService : IStockService
{
    private readonly ILocationsRepository _locationsRepository;
    private readonly IItemsRepository _itemsRepository;
    private readonly IStockRepository _stockRepository;

    public StockService(ILocationsRepository locationsRepository, IItemsRepository itemsRepository,
        IStockRepository stockRepository)
    {
        _locationsRepository = locationsRepository;
        _itemsRepository = itemsRepository;
        _stockRepository = stockRepository;
    }

    public async Task<PageResult<Stock>> GetStockPageAsync(Guid locationId, PageRequest pageRequest)
    {
        await _locationsRepository.Exists(locationId);

        return await _stockRepository.GetPageAsync(locationId, pageRequest);
    }

    public async Task<Stock?> GetStockForItemAsync(Guid locationId, Guid itemId)
    {
        await _locationsRepository.Exists(locationId);
        await _itemsRepository.Exists(itemId);

        return await _stockRepository.GetAsync(locationId, itemId);
    }

    public async Task<Stock> UpdateStockForItemAsync(Guid locationId, Guid itemId, Stock stock)
    {
        await _locationsRepository.Exists(locationId);
        await _itemsRepository.Exists(itemId);

        var existingStock = await _stockRepository.GetAsync(locationId, itemId);
        existingStock.Update(stock);
        await _stockRepository.SaveChangesAsync();

        return existingStock;
    }
}