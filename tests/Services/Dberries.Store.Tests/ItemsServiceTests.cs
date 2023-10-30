using BitzArt;
using BitzArt.ApiExceptions;
using BitzArt.Pagination;
using Microsoft.Extensions.DependencyInjection;

namespace Dberries.Store.Tests;

[Collection("Service Collection")]
public class ItemsServiceTests
{
    private readonly IItemsService _itemsService;

    public ItemsServiceTests(TestServiceContainer testServiceContainer)
    {
        var serviceProvider = testServiceContainer.ServiceProvider;
        _itemsService = serviceProvider.GetRequiredService<IItemsService>();
    }

    [Fact]
    public async Task GetItemsPage_ExistingItems_ItemsReceived()
    {
        // Arrange
        const int itemsCount = 10;

        for (var i = 0; i < itemsCount; i++)
        {
            var item = EntityGenerator.GenerateItem();
            await _itemsService.AddAsync(item);
        }

        var pageRequest = new PageRequest(0, itemsCount);

        // Act
        var itemsPage = await _itemsService.GetPageAsync(pageRequest);

        // Assert
        Assert.NotNull(itemsPage);
        Assert.NotNull(itemsPage.Data);
        Assert.Equal(itemsCount, itemsPage.Data.Count());
    }

    [Fact]
    public async Task GetItem_ExistingItem_ItemReceived()
    {
        // Arrange
        var item = EntityGenerator.GenerateItem();
        await _itemsService.AddAsync(item);

        // Act
        var receivedItem = await _itemsService.GetAsync(item.Id!.Value);

        // Assert
        Assert.NotNull(receivedItem);
        Assert.Equal(item.Id, receivedItem.Id);
        Assert.Equal(item.ExternalId, receivedItem.ExternalId);
        Assert.Equal(item.Name, receivedItem.Name);
        Assert.Equal(item.Description, receivedItem.Description);
    }

    [Fact]
    public async Task GetItem_NotExistingItem_NotFoundApiExceptionThrown()
    {
        // Arrange
        var itemId = Guid.NewGuid();
        Task Action() => _itemsService.GetAsync(itemId);

        // Assert
        await Assert.ThrowsAsync<NotFoundApiException>(Action);
    }

    [Fact]
    public async Task AddItem_NewItem_ItemAdded()
    {
        // Arrange
        var item = EntityGenerator.GenerateItem();

        // Act
        var addedItem = await _itemsService.AddAsync(item);

        // Assert
        Assert.NotNull(addedItem);
        Assert.Equal(item.Name, addedItem.Name);
        Assert.Equal(item.Description, addedItem.Description);
    }

    [Fact]
    public async Task AddItem_ExistingItem_BadRequestApiExceptionThrown()
    {
        // Arrange
        var item = EntityGenerator.GenerateItem();
        await _itemsService.AddAsync(item);
        Task Action() => _itemsService.AddAsync(item);

        // Assert
        await Assert.ThrowsAsync<BadRequestApiException>(Action);
    }

    [Fact]
    public async Task UpdateItem_ExistingItem_ItemUpdated()
    {
        // Arrange
        var item = EntityGenerator.GenerateItem();
        item = await _itemsService.AddAsync(item);

        var payload = new Item
        {
            Name = "Updated Name",
            Description = "Updated Description"
        };

        item.Patch(payload)
            .Property(x => x.Name)
            .Property(x => x.Description);

        // Act
        var updatedItem = await _itemsService.UpdateAsync(item);

        // Assert
        Assert.NotNull(updatedItem);
        Assert.Equal(item.Id, updatedItem.Id);
        Assert.Equal(item.ExternalId, updatedItem.ExternalId);
        Assert.Equal(item.Name, updatedItem.Name);
        Assert.Equal(item.Description, updatedItem.Description);
    }

    [Fact]
    public async Task UpdateItem_NotExistingItem_ItemAdded()
    {
        // Arrange
        var item = new Item
        {
            Id = Guid.NewGuid(),
            ExternalId = Guid.NewGuid(),
            Name = "Updated Name",
            Description = "Updated Description"
        };

        // Act
        item = await _itemsService.UpdateAsync(item);

        // Assert
        var addedItem = await _itemsService.GetAsync(item.Id!.Value);

        Assert.NotNull(addedItem);
        Assert.Equal(item.Id, addedItem.Id);
        Assert.Equal(item.ExternalId, addedItem.ExternalId);
        Assert.Equal(item.Name, addedItem.Name);
        Assert.Equal(item.Description, addedItem.Description);
    }

    [Fact]
    public async Task RemoveItem_ExistingItem_ItemRemoved()
    {
        // Arrange
        var item = EntityGenerator.GenerateItem();
        item = await _itemsService.AddAsync(item);

        Task Action() => _itemsService.GetAsync(item.Id!.Value);

        // Act
        await _itemsService.RemoveAsync(item.ExternalId!.Value);

        // Assert
        await Assert.ThrowsAsync<NotFoundApiException>(Action);
    }
}