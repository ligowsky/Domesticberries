using Dberries.Warehouse;
using MassTransit;

namespace Dberries.Store.WebAPI;

public class ItemUpdatedMessageConsumer : IConsumer<ItemUpdatedMessage>
{
    private readonly IItemsService _itemsService;

    public ItemUpdatedMessageConsumer(IItemsService itemsService)
    {
        _itemsService = itemsService;
    }

    public async Task Consume(ConsumeContext<ItemUpdatedMessage> context)
    {
        var item = context.Message.Item.ToModel();
        await _itemsService.UpdateAsync(item.ExternalId!.Value, item);
    }
}