using BitzArt;
using BitzArt.ApiExceptions;
using BitzArt.Pagination;
using Microsoft.Extensions.DependencyInjection;

namespace Dberries.Store.Tests;

[Collection("Service Collection")]
public class ItemsServiceTests
{
    private readonly IItemsService _itemsService;
    private readonly ILocationsService _locationsService;
    private readonly AppDbContext _db;

    public ItemsServiceTests(TestServiceContainer testServiceContainer)
    {
        var serviceProvider = testServiceContainer.ServiceProvider;
        _itemsService = serviceProvider.GetRequiredService<IItemsService>();
        _locationsService = serviceProvider.GetRequiredService<ILocationsService>();
        _db = serviceProvider.GetRequiredService<AppDbContext>();
    }

    [Fact]
    public async Task GetItemsPage_ExistingItems_ReturnsItemsPage()
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
    public async Task GetItem_ExistingItem_ReturnsItem()
    {
        // Arrange
        var item = EntityGenerator.GenerateItem();
        await _itemsService.AddAsync(item);

        // Act
        var returnedItem = await _itemsService.GetAsync(item.Id!.Value);

        // Assert
        Assert.NotNull(returnedItem);
        Assert.Equal(item.Id, returnedItem.Id);
        Assert.Equal(item.ExternalId, returnedItem.ExternalId);
        Assert.Equal(item.Name, returnedItem.Name);
        Assert.Equal(item.Description, returnedItem.Description);
    }

    [Fact]
    public async Task GetItem_NotExistingItem_ThrowsNotFoundApiException()
    {
        // Arrange
        var itemId = Guid.NewGuid();

        // Assert
        Task Action() => _itemsService.GetAsync(itemId);
        await Assert.ThrowsAsync<NotFoundApiException>(Action);
    }

    [Fact]
    public async Task AddItem_NewItem_AddsItem()
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
    public async Task AddItem_ExistingItem_ThrowsBadRequestApiException()
    {
        // Arrange
        var item = EntityGenerator.GenerateItem();
        await _itemsService.AddAsync(item);

        // Assert
        Task Action() => _itemsService.AddAsync(item);
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
    public async Task UpdateItem_NotExistingItem_AddsItem()
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
    public async Task RemoveItem_ExistingItem_RemovesItem()
    {
        // Arrange
        var item = EntityGenerator.GenerateItem();
        item = await _itemsService.AddAsync(item);

        // Act
        await _itemsService.RemoveAsync(item.ExternalId!.Value);

        // Assert
        Task Action() => _itemsService.GetAsync(item.Id!.Value);
        await Assert.ThrowsAsync<NotFoundApiException>(Action);
    }

    [Fact]
    public async Task RemoveItem_NotExistingItem_DoesNotThrowNotFoundApiException()
    {
        // Arrange
        var itemId = Guid.NewGuid();

        // Assert
        var exception = await Record.ExceptionAsync(() => _itemsService.RemoveAsync(itemId));
        Assert.Null(exception);
    }

    [Fact]
    public async Task GetAvailability_ExistingItem_ReturnsItemAvailabilityList()
    {
        // Arrange
        var item = EntityGenerator.GenerateItem();
        item = await _itemsService.AddAsync(item);

        const int locationCount = 10;
        const int itemsQuantity = 5;

        var locations = Enumerable.Range(0, locationCount)
            .Select(EntityGenerator.GenerateLocation).ToList();

        foreach (var location in locations)
        {
            await _locationsService.AddAsync(location);

            var stock = EntityGenerator.GenerateStock(itemsQuantity);
            await _locationsService.UpdateStockAsync(location.ExternalId!.Value, item.ExternalId!.Value,
                stock.Quantity!.Value);

            _db.ChangeTracker.Clear();
        }

        // Assert
        var itemAvailability = await _itemsService.GetAvailabilityAsync(item.Id!.Value);
        var availabilityInLocations = itemAvailability.AvailableInLocations.ToList();

        Assert.NotNull(itemAvailability);
        Assert.Equal(locationCount, availabilityInLocations.Count);

        foreach (var availabilityInLocation in availabilityInLocations)
        {
            Assert.Contains(locations, x => x.Id == availabilityInLocation.LocationId);
            Assert.Equal(availabilityInLocation.Quantity, itemsQuantity);
        }
    }

    [Fact]
    public async Task GetAvailability_NotExistingItem_ThrowsNotFoundApiException()
    {
        // Arrange
        var itemId = Guid.NewGuid();

        // Assert
        Task Action() => _itemsService.GetAvailabilityAsync(itemId);
        await Assert.ThrowsAsync<NotFoundApiException>(Action);
    }

    [Fact]
    public async Task UpdateRating_ExistingItem_UpdatedItemRating()
    {
        // Arrange
        var item = EntityGenerator.GenerateItem();
        item = await _itemsService.AddAsync(item);
        var random = new Random();
        var ratingCount = random.Next(1, 10);

        var ratings = Enumerable.Range(0, ratingCount)
            .Select(_ =>
            {
                var value = (byte)random.Next(1, 5);
                return EntityGenerator.GenerateRating(value);
            }).ToList();

        var averageRating = ratings.Average(x => x.Value)!;

        // Act 
        foreach (var rating in ratings)
        {
            await _itemsService.UpdateRatingAsync(item.Id!.Value, rating);
        }

        // Assert
        var updatedItem = await _itemsService.GetAsync(item.Id!.Value);

        Assert.NotNull(updatedItem);
        Assert.Equal(item.Id, updatedItem.Id);
        Assert.Equal(averageRating, updatedItem.AverageRating);
    }

    [Fact]
    public async Task UpdateRating_ZeroValue_RemovesRating()
    {
        // Arrange
        var item = EntityGenerator.GenerateItem();
        item = await _itemsService.AddAsync(item);
        var rating = EntityGenerator.GenerateRating(5);
        await _itemsService.UpdateRatingAsync(item.Id!.Value, rating);
        rating.Value = 0;

        // Assert
        var updatedItem = await _itemsService.UpdateRatingAsync(item.Id!.Value, rating);
        var existingRating = updatedItem.Ratings!.FirstOrDefault(x => x.UserId == rating.UserId);

        Assert.NotNull(updatedItem);
        Assert.Equal(item.Id, updatedItem.Id);
        Assert.Null(existingRating);
    }

    [Theory]
    [InlineData(-1)]
    [InlineData(6)]
    public async Task UpdateRating_InvalidValue_ThrowsBadRequestApiException(int value)
    {
        // Arrange
        var item = EntityGenerator.GenerateItem();
        item = await _itemsService.AddAsync(item);
        var rating = EntityGenerator.GenerateRating();
        rating.Value = (byte)value;

        // Assert
        Task Action() => _itemsService.UpdateRatingAsync(item.Id!.Value, rating);
        await Assert.ThrowsAsync<BadRequestApiException>(Action);
    }

    [Fact]
    public async Task UpdateRating_NotExistingItem_ThrowsNotFoundApiException()
    {
        // Arrange
        var itemId = Guid.NewGuid();
        var rating = EntityGenerator.GenerateRating();

        // Assert
        Task Action() => _itemsService.UpdateRatingAsync(itemId, rating);
        await Assert.ThrowsAsync<NotFoundApiException>(Action);
    }
}