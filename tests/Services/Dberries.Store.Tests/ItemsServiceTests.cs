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
    private readonly IUsersService _usersService;
    private readonly AppDbContext _db;
    private readonly Random _random;

    public ItemsServiceTests(TestServiceContainer testServiceContainer)
    {
        var serviceProvider = testServiceContainer.ServiceProvider;
        _itemsService = serviceProvider.GetRequiredService<IItemsService>();
        _locationsService = serviceProvider.GetRequiredService<ILocationsService>();
        _usersService = serviceProvider.GetRequiredService<IUsersService>();
        _db = serviceProvider.GetRequiredService<AppDbContext>();
        _random = new Random();
    }

    [Fact]
    public async Task GetItemsPage_ExistingItems_ReturnsItemsPage()
    {
        // Arrange
        const int offset = 0;
        var limit = _random.Next(1, 10);
        var pageRequest = new PageRequest(offset, limit);

        for (var i = 0; i < limit; i++)
        {
            var item = EntityGenerator.GenerateItem();
            await _itemsService.AddAsync(item);
        }

        // Act
        var itemsPage = await _itemsService.GetPageAsync(pageRequest);

        // Assert
        Assert.NotNull(itemsPage);
        Assert.NotNull(itemsPage.Data);
        Assert.Equal(limit, itemsPage.Data.Count());
    }

    [Fact]
    public async Task GetItem_ExistingItem_ReturnsItem()
    {
        // Arrange
        var item = EntityGenerator.GenerateItem();
        item = await _itemsService.AddAsync(item);
        var filter = new ItemFilterSet { Id = item.Id };

        // Act
        var returnedItem = await _itemsService.GetAsync(filter);

        // Assert
        Assert.NotNull(returnedItem);
        Assert.Equal(item.Id, returnedItem.Id);
        Assert.Equal(item.ExternalId, returnedItem.ExternalId);
        Assert.Equal(item.Name, returnedItem.Name);
        Assert.Equal(item.Description, returnedItem.Description);
        Assert.NotNull(returnedItem.AverageRating);
    }

    [Fact]
    public async Task GetItem_NotExistingItem_ThrowsNotFoundApiException()
    {
        // Arrange
        var filter = new ItemFilterSet { Id = Guid.NewGuid() };

        // Assert
        Task Action() => _itemsService.GetAsync(filter);
        await Assert.ThrowsAsync<NotFoundApiException>(Action);
    }

    [Fact]
    public async Task AddItem_NewItem_AddsItem()
    {
        // Arrange
        var item = EntityGenerator.GenerateItem();

        // Act
        item = await _itemsService.AddAsync(item);

        // Assert
        var filter = new ItemFilterSet { Id = item.Id };
        var addedItem = await _itemsService.GetAsync(filter);

        Assert.NotNull(addedItem);
        Assert.Equal(item.Id, addedItem.Id);
        Assert.Equal(item.ExternalId, addedItem.ExternalId);
        Assert.Equal(item.Name, addedItem.Name);
        Assert.Equal(item.Description, addedItem.Description);
    }

    [Fact]
    public async Task AddItem_ExistingItem_ThrowsConflictApiException()
    {
        // Arrange
        var item = EntityGenerator.GenerateItem();
        await _itemsService.AddAsync(item);

        // Assert
        Task Action() => _itemsService.AddAsync(item);
        await Assert.ThrowsAsync<ConflictApiException>(Action);
    }

    [Fact]
    public async Task UpdateItem_ExistingItem_ItemUpdated()
    {
        // Arrange
        var item = EntityGenerator.GenerateItem();
        item = await _itemsService.AddAsync(item);

        var payload = new Item("Updated name", "Updated Description");

        item.Patch(payload)
            .Property(x => x.Name)
            .Property(x => x.Description);

        // Act
        item = await _itemsService.UpdateAsync(item.ExternalId!.Value, item);

        // Assert
        var filter = new ItemFilterSet { Id = item.Id };
        var updatedItem = await _itemsService.GetAsync(filter);

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
        var item = EntityGenerator.GenerateItem();

        // Act
        item = await _itemsService.UpdateAsync(item.ExternalId!.Value, item);

        // Assert
        var filter = new ItemFilterSet { Id = item.Id };
        var addedItem = await _itemsService.GetAsync(filter);

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
        var filter = new ItemFilterSet { Id = item.Id };
        Task Action() => _itemsService.GetAsync(filter);

        await Assert.ThrowsAsync<NotFoundApiException>(Action);
    }

    [Fact]
    public async Task RemoveItem_NotExistingItem_DoesNotThrowException()
    {
        // Arrange
        var id = Guid.NewGuid();

        // Assert
        Task Action() => _itemsService.RemoveAsync(id);
        var exception = await Record.ExceptionAsync(Action);
        Assert.Null(exception);
    }

    [Fact]
    public async Task GetAvailability_ExistingItem_ReturnsItemAvailabilityList()
    {
        // Arrange
        var item = EntityGenerator.GenerateItem();
        item = await _itemsService.AddAsync(item);

        var locationsCount = _random.Next(1, 10);
        var itemsQuantity = _random.Next(1, 5);

        var locations = Enumerable.Range(0, locationsCount)
            .Select(_ => EntityGenerator.GenerateLocation()).ToList();

        foreach (var location in locations)
        {
            await _locationsService.AddAsync(location);

            await _locationsService.UpdateStockAsync(location.ExternalId!.Value, item.ExternalId!.Value, itemsQuantity);

            _db.ChangeTracker.Clear();
        }

        // Act 
        var itemAvailability = await _itemsService.GetAvailabilityAsync(item.Id!.Value);

        // Assert
        var availabilityInLocations = itemAvailability.AvailableInLocations.ToList();

        Assert.NotNull(itemAvailability);
        Assert.Equal(locationsCount, availabilityInLocations.Count);

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
    public async Task UpdateRating_ExistingItem_UpdatesItemRating()
    {
        // Arrange
        var item = EntityGenerator.GenerateItem();
        item = await _itemsService.AddAsync(item);

        var ratingCount = _random.Next(1, 10);

        var ratingUpdates = Enumerable.Range(0, ratingCount)
            .Select(async _ =>
            {
                var user = EntityGenerator.GenerateUser();
                user = await _usersService.AddAsync(user);

                var value = (byte)_random.Next(1, 5);

                return (UserId: user.ExternalId, Value: value);
            })
            .Select(x => x.Result).ToList();

        var averageRating = Math.Round((decimal)ratingUpdates.Average(x => x.Value), 2);

        // Act 
        foreach (var ratingUpdate in ratingUpdates)
        {
            await _itemsService.UpdateRatingAsync(item.Id!.Value, ratingUpdate.UserId!.Value, ratingUpdate.Value);
        }

        var itemFilter = new ItemFilterSet { Id = item.Id };

        // Assert
        var returnedItem = await _itemsService.GetAsync(itemFilter);

        Assert.NotNull(returnedItem);
        Assert.Equal(item.Id, returnedItem.Id);
        Assert.Equal(item.ExternalId, returnedItem.ExternalId);
        Assert.NotNull(returnedItem.AverageRating);
        Assert.Equal(averageRating, returnedItem.AverageRating);
    }

    [Fact]
    public async Task UpdateRating_ExistingRating_UpdatedItemRating()
    {
        // Arrange
        var item = EntityGenerator.GenerateItem();
        item = await _itemsService.AddAsync(item);

        var user = EntityGenerator.GenerateUser();
        await _usersService.AddAsync(user);

        var value = (byte)_random.Next(1, 5);

        await _itemsService.UpdateRatingAsync(item.Id!.Value, user.ExternalId!.Value, value);

        value = (byte)_random.Next(1, 5);

        var itemFilter = new ItemFilterSet { Id = item.Id };

        // Act 
        await _itemsService.UpdateRatingAsync(item.Id!.Value, user.ExternalId!.Value, value);

        // Assert
        var returnedItem = await _itemsService.GetAsync(itemFilter);

        Assert.NotNull(returnedItem);
        Assert.Equal(item.Id, returnedItem.Id);
        Assert.Equal(item.ExternalId, returnedItem.ExternalId);
        Assert.NotNull(returnedItem.AverageRating);
        Assert.Equal(value, returnedItem.AverageRating);
    }

    [Fact]
    public async Task UpdateRating_NotExistingItem_ThrowsNotFoundApiException()
    {
        // Arrange
        var itemId = Guid.NewGuid();

        var user = EntityGenerator.GenerateUser();
        await _usersService.AddAsync(user);

        var value = (byte)_random.Next(1, 5);

        // Assert
        Task Action() => _itemsService.UpdateRatingAsync(itemId, user.ExternalId!.Value, value);
        await Assert.ThrowsAsync<NotFoundApiException>(Action);
    }

    [Fact]
    public async Task UpdateRating_NotExistingUser_ThrowsNotFoundApiException()
    {
        // Arrange
        var item = EntityGenerator.GenerateItem();
        item = await _itemsService.AddAsync(item);

        var userId = Guid.NewGuid();

        var value = (byte)_random.Next(1, 5);

        // Assert
        Task Action() => _itemsService.UpdateRatingAsync(item.ExternalId!.Value, userId, value);
        await Assert.ThrowsAsync<NotFoundApiException>(Action);
    }

    [Fact]
    public async Task RemoveRating_ExistingItem_RemovesRating()
    {
        // Arrange
        var item = EntityGenerator.GenerateItem();
        item = await _itemsService.AddAsync(item);

        var user = EntityGenerator.GenerateUser();
        user = await _usersService.AddAsync(user);

        var value = (byte)_random.Next(1, 5);

        await _itemsService.UpdateRatingAsync(item.Id!.Value, user.ExternalId!.Value, value);

        // Assert
        var updatedItem = await _itemsService.RemoveRatingAsync(item.Id!.Value, user.ExternalId!.Value);
        var ratingExists = updatedItem.Ratings!.Any(x => x.UserId == user.Id);

        Assert.NotNull(updatedItem);
        Assert.Equal(item.Id, updatedItem.Id);
        Assert.Equal(item.ExternalId, updatedItem.ExternalId);
        Assert.False(ratingExists);
    }

    [Fact]
    public async Task RemoveRating_NotExistingItem_ThrowsNotFoundApiException()
    {
        // Arrange
        var itemId = Guid.NewGuid();

        var user = EntityGenerator.GenerateUser();
        await _usersService.AddAsync(user);

        // Assert
        Task Action() => _itemsService.RemoveRatingAsync(itemId, user.ExternalId!.Value);
        await Assert.ThrowsAsync<NotFoundApiException>(Action);
    }

    [Fact]
    public async Task RemoveRating_NotExistingUser_ThrowsNotFoundApiException()
    {
        // Arrange
        var item = EntityGenerator.GenerateItem();
        item = await _itemsService.AddAsync(item);

        var userId = Guid.NewGuid();

        // Assert
        Task Action() => _itemsService.RemoveRatingAsync(item.Id!.Value, userId);
        await Assert.ThrowsAsync<NotFoundApiException>(Action);
    }
}