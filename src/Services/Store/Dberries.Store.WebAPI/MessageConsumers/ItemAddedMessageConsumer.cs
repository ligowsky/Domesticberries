using Dberries.Warehouse;
using MassTransit;

namespace Dberries.Store.WebAPI;

public class ItemAddedMessageConsumer : IConsumer<ItemAddedMessage>
{
    private readonly IItemsRepository _itemsRepository;

    public ItemAddedMessageConsumer(IItemsRepository itemsRepository)
    {
        _itemsRepository = itemsRepository;
    }

    public async Task Consume(ConsumeContext<ItemAddedMessage> context)
    {
        var item = context.Message.Item.ToModel();

        var x = await _itemsRepository.CheckExistsByExternalIdAsync(typeof(Item), item.ExternalId!.Value, true);
        _itemsRepository.AddAsync(item);
        await _itemsRepository.SaveChangesAsync();
    }
}