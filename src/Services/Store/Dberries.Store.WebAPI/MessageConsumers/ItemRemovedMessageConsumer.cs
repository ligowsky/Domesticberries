using Dberries.Warehouse;
using MassTransit;

namespace Dberries.Store.WebAPI;

public class ItemRemovedMessageConsumer : IConsumer<ItemRemovedMessage>
{
    private readonly IItemsService _itemsService;

    public ItemRemovedMessageConsumer(IItemsService itemsService)
    {
        _itemsService = itemsService;
    }

    public async Task Consume(ConsumeContext<ItemRemovedMessage> context)
    {
        var id = context.Message.Id;
        await _itemsService.RemoveAsync(id);
    }
}