using Dberries.Warehouse;
using MassTransit;

namespace Dberries.Store.WebAPI;

public class ItemAddedMessageConsumer : IConsumer<ItemAddedMessage>
{
    private readonly IItemsService _itemsService;

    public ItemAddedMessageConsumer(IItemsService itemsService)
    {
        _itemsService = itemsService;
    }

    public async Task Consume(ConsumeContext<ItemAddedMessage> context)
    {
        var item = context.Message.Item.ToModel();
        await _itemsService.AddAsync(item);
    }
}