using BitzArt;
using Dberries.Warehouse;
using MassTransit;

namespace Dberries.Store.WebAPI;

public class ItemUpdatedMessageConsumer : IConsumer<ItemUpdatedMessage>
{
    private readonly IItemsRepository _itemsRepository;
    
    public ItemUpdatedMessageConsumer(IItemsRepository itemsRepository)
    {
        _itemsRepository = itemsRepository;
    }
    
    public async Task Consume(ConsumeContext<ItemUpdatedMessage> context)
    {
        var item = context.Message.Item.ToModel();

        var existingItem = await _itemsRepository.GetByExternalIdAsync(item.ExternalId!.Value, false);

        if (existingItem is null)
        {
            _itemsRepository.AddAsync(item);
        }
        else
        {
            existingItem.Patch(item)
                .Property(x => x.Name)
                .Property(x => x.Description);
        }
        
        await _itemsRepository.SaveChangesAsync();
    }
}