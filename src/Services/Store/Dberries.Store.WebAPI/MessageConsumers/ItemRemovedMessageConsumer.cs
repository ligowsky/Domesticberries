using Dberries.Warehouse;
using MassTransit;

namespace Dberries.Store.WebAPI;

public class ItemRemovedMessageConsumer : IConsumer<ItemRemovedMessage>
{
    private readonly IItemsRepository _itemsRepository;

    public ItemRemovedMessageConsumer(IItemsRepository itemsRepository)
    {
        _itemsRepository = itemsRepository;
    }

    public async Task Consume(ConsumeContext<ItemRemovedMessage> context)
    {
        var id = context.Message.Id;

        var existingItem = await _itemsRepository.GetByExternalIdAsync(id, false);

        if (existingItem is null) return;
        
        _itemsRepository.Remove(existingItem);
        await _itemsRepository.SaveChangesAsync();
    }
}