using BitzArt;
using BitzArt.ApiExceptions;
using BitzArt.Pagination;
using Microsoft.Extensions.DependencyInjection;

namespace Dberries.Warehouse.Tests;

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
            var item = GenerateItem();
            await _itemsService.AddAsync(item);
        }

        var pageRequest = new PageRequest()
        {
            Offset = 0,
            Limit = itemsCount
        };

        // Act
        var itemsPage = await _itemsService.GetPageAsync(pageRequest);

        // Assert
        Assert.NotNull(itemsPage);
        Assert.Equal(itemsCount, itemsPage.Data!.Count());
    }

    [Fact]
    public async Task GetItem_ExistingItem_ItemReceived()
    {
        // Arrange
        var item = GenerateItem();
        await _itemsService.AddAsync(item);

        // Act
        var existingItem = await _itemsService.GetAsync(item.Id!.Value);

        // Assert
        Assert.NotNull(existingItem);
        Assert.Equal(item.Id, existingItem.Id);
        Assert.Equal(item.Name, existingItem.Name);
        Assert.Equal(item.Description, existingItem.Description);
    }

    [Fact]
    public async Task GetItem_NotExistingItem_NotFoundApiExceptionThrown()
    {
        // Arrange
        var itemId = Guid.NewGuid();
        var exceptionType = typeof(NotFoundApiException);
        var expectedMessage = $"{nameof(Item)} with id '{itemId}' is not found";

        // Act
        var exception =
            await Record.ExceptionAsync(async () => await _itemsService.GetAsync(itemId));

        // Assert
        Assert.IsType(exceptionType, exception);
        Assert.Equal(expectedMessage, exception.Message);
    }

    [Fact]
    public async Task AddItem_NewItem_ItemAdded()
    {
        // Arrange
        var item = GenerateItem();

        // Act
        await _itemsService.AddAsync(item);

        // Assert
        var createdItem = await _itemsService.GetAsync(item.Id!.Value);

        Assert.NotNull(createdItem);
        Assert.Equal(item.Name, createdItem.Name);
        Assert.Equal(item.Description, createdItem.Description);
    }

    [Theory]
    [InlineData("Item 2", "Description 2")]
    [InlineData("Item 3", "Description 3")]
    [InlineData("Item 4", "Description 4")]
    public async Task UpdateItem_ExistingItem_ItemUpdated(string name, string description)
    {
        // Arrange
        var newItem = GenerateItem();
        await _itemsService.AddAsync(newItem);
        var existingItem = await _itemsService.GetAsync(newItem.Id!.Value);

        var item = new Item
        {
            Name = name,
            Description = description
        };

        // Act
        existingItem.Patch(item)
            .Property(x => x.Name)
            .Property(x => x.Description);

        await _itemsService.UpdateAsync(existingItem.Id!.Value, existingItem);

        // Assert
        var updatedItem = await _itemsService.GetAsync(existingItem.Id!.Value);

        Assert.NotNull(updatedItem);
        Assert.Equal(existingItem.Id, updatedItem.Id);
        Assert.Equal(updatedItem.Name, name);
        Assert.Equal(updatedItem.Description, description);
    }

    [Fact]
    public void UpdateItem_NotExistingItem_NotFoundApiExceptionThrown()
    {
        // Arrange
        var itemId = Guid.NewGuid();
        var item = GenerateItem();
        var exceptionType = typeof(NotFoundApiException);
        var expectedMessage = $"{nameof(Item)} with id '{itemId}' is not found";

        // Act
        var exception =
            Record.ExceptionAsync(async () => await _itemsService.UpdateAsync(itemId, item));

        // Assert
        Assert.IsType(exceptionType, exception.Result);
        Assert.Equal(expectedMessage, exception.Result.Message);
    }

    [Fact]
    public async Task RemoveItem_ExistingItem_ItemRemoved()
    {
        // Arrange
        var newItem = GenerateItem();
        await _itemsService.AddAsync(newItem);
        var existingItem = await _itemsService.GetAsync(newItem.Id!.Value);
        var exceptionType = typeof(NotFoundApiException);
        var expectedMessage = $"{nameof(Item)} with id '{existingItem.Id}' is not found";

        // Act
        await _itemsService.RemoveAsync(existingItem.Id!.Value);

        // Assert
        var exception =
            await Record.ExceptionAsync(
                async () => await _itemsService.GetAsync(existingItem.Id!.Value));

        Assert.IsType(exceptionType, exception);
        Assert.Equal(expectedMessage, exception.Message);
    }

    [Fact]
    public void RemoveItem_NotExistingItem_NotFoundApiExceptionThrown()
    {
        // Arrange
        var itemId = Guid.NewGuid();
        var exceptionType = typeof(NotFoundApiException);
        var expectedMessage = $"{nameof(Item)} with id '{itemId}' is not found";

        // Act
        var exception =
            Record.ExceptionAsync(async () => await _itemsService.RemoveAsync(itemId));

        // Assert
        Assert.IsType(exceptionType, exception.Result);
        Assert.Equal(expectedMessage, exception.Result.Message);
    }

    private Item GenerateItem(int number = 1)
    {
        return new Item
        {
            Name = $"Item {number}",
            Description = $"Description {number}"
        };
    }
}