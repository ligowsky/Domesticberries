using BitzArt;
using Dberries.Warehouse;
using MassTransit;

namespace Dberries.Store.Infrastructure;

public class ItemUpdatedMessageConsumer : IConsumer<ItemUpdatedMessage>
{
    private readonly IItemsRepository _itemsRepository;
    
    public ItemUpdatedMessageConsumer(IItemsRepository itemsRepository)
    {
        _itemsRepository = itemsRepository;
    }
    
    public async Task Consume(ConsumeContext<ItemUpdatedMessage> context)
    {
        var message = context.Message;
        var item = message.Item.ToModel();

        var existingItem = await _itemsRepository.GetByExternalIdAsync(item.ExternalId!.Value);

        existingItem.Patch(item)
            .Property(x => x.Name)
            .Property(x => x.Description);

        await _itemsRepository.SaveChangesAsync();
    }
}