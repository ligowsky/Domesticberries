using Dberries.Warehouse;
using MassTransit;

namespace Dberries.Store;

public class ItemAddedMessageConsumer : IConsumer<ItemAddedMessage>
{
    private readonly IItemsRepository _itemsRepository;
    
    public ItemAddedMessageConsumer(IItemsRepository itemsRepository)
    {
        _itemsRepository = itemsRepository;
    }
    
    public async Task Consume(ConsumeContext<ItemAddedMessage> context)
    {
        var message = context.Message;
        var item = message.Item.ToModel();

        _itemsRepository.Add(item);
        await _itemsRepository.SaveChangesAsync();
    }
}