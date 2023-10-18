using MassTransit.Testing;
using Microsoft.Extensions.DependencyInjection;

namespace Dberries.Warehouse.Tests;

[Collection("Service Collection")]
public class ItemsMessagesTests
{
    private readonly IItemsService _itemsService;
    private readonly ITestHarness _harness;

    public ItemsMessagesTests(TestServiceContainer testServiceContainer)
    {
        var serviceProvider = testServiceContainer.ServiceProvider;
        _itemsService = serviceProvider.GetRequiredService<IItemsService>();
        _harness = serviceProvider.GetRequiredService<ITestHarness>();
    }

    [Fact]
    public async Task AddItem_NewItem_MessagePublished()
    {
        // Arrange
        var item = EntityGenerator.GenerateItem();

        // Act
        item = await _itemsService.AddAsync(item);

        // Assert
        var message = _harness.Published
            .Select<ItemAddedMessage>()
            .FirstOrDefault(x => x.Context.Message.Item.Id == item.Id)?
            .Context.Message;

        Assert.NotNull(message);
        Assert.NotNull(message.Item);
        Assert.Equal(item.Id, message.Item.Id);
        Assert.Equal(item.Name, message.Item.Name);
    }

    [Fact]
    public async Task UpdateItem_ExistingItem_MessagePublished()
    {
        // Arrange
        var item = EntityGenerator.GenerateItem();
        item = await _itemsService.AddAsync(item);

        // Act
        item = await _itemsService.UpdateAsync(item.Id!.Value, item);

        // Assert
        var message = _harness.Published
            .Select<ItemUpdatedMessage>()
            .FirstOrDefault(x => x.Context.Message.Item.Id == item.Id)?
            .Context.Message;

        Assert.NotNull(message);
        Assert.NotNull(message.Item);
        Assert.Equal(item.Id, message.Item.Id);
        Assert.Equal(item.Name, message.Item.Name);
    }

    [Fact]
    public async Task UpdateItem_NotExistingItem_MessageNotPublished()
    {
        // Arrange
        var itemId = Guid.NewGuid();
        var item = EntityGenerator.GenerateItem();

        // Assert
        var exception =
            await Record.ExceptionAsync(async () => await _itemsService.UpdateAsync(itemId, item));

        var message = _harness.Published
            .Select<ItemUpdatedMessage>()
            .FirstOrDefault(x => x.Context.Message.Item.Id == item.Id)?.Context.Message;

        Assert.NotNull(exception);
        Assert.Null(message);
    }

    [Fact]
    public async Task RemoveItem_ExistingItem_MessagePublished()
    {
        // Arrange
        var item = EntityGenerator.GenerateItem();
        item = await _itemsService.AddAsync(item);

        // Act
        await _itemsService.RemoveAsync(item.Id!.Value);

        // Assert
        var message = _harness.Published
            .Select<ItemRemovedMessage>()
            .FirstOrDefault(x => x.Context.Message.Id == item.Id)?
            .Context.Message;

        Assert.NotNull(message);
        Assert.Equal(item.Id, message.Id);
    }

    [Fact]
    public async Task RemoveItem_NotExistingItem_MessageNotPublished()
    {
        // Arrange
        var itemId = Guid.NewGuid();

        // Assert
        var exception =
            await Record.ExceptionAsync(async () => await _itemsService.RemoveAsync(itemId));

        var message = _harness.Published
            .Select<ItemRemovedMessage>()
            .FirstOrDefault(x => x.Context.Message.Id == itemId)?.Context.Message;

        Assert.NotNull(exception);
        Assert.Null(message);
    }
}