using Dberries.Warehouse;
using MassTransit;

namespace Dberries.Store;

public class ItemRemovedMessageConsumer : IConsumer<ItemRemovedMessage>
{
    private readonly IItemsRepository _itemsRepository;
    
    public ItemRemovedMessageConsumer(IItemsRepository itemsRepository)
    {
        _itemsRepository = itemsRepository;
    }
    
    public async Task Consume(ConsumeContext<ItemRemovedMessage> context)
    {
        var message = context.Message;
        var id = message.Id;

        var existingItem = await _itemsRepository.GetAsync(id);
        _itemsRepository.Remove(existingItem);
        await _itemsRepository.SaveChangesAsync();
    }
}