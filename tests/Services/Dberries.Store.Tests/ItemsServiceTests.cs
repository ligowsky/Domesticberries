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
            var filter = new ItemFilterSet { ExternalId = item.ExternalId };
            await _itemsService.AddAsync(filter, item);
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
        var filter = new ItemFilterSet { ExternalId = item.ExternalId };
        item = await _itemsService.AddAsync(filter, item);
        filter = new ItemFilterSet { Id = item.Id };

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
        var filter = new ItemFilterSet { ExternalId = item.ExternalId };

        // Act
        item = await _itemsService.AddAsync(filter, item);

        // Assert
        filter = new ItemFilterSet { Id = item.Id };
        var returnedItem = await _itemsService.GetAsync(filter);

        Assert.NotNull(returnedItem);
        Assert.Equal(item.Id, returnedItem.Id);
        Assert.Equal(item.ExternalId, returnedItem.ExternalId);
        Assert.Equal(item.Name, returnedItem.Name);
        Assert.Equal(item.Description, returnedItem.Description);
        Assert.NotNull(returnedItem.AverageRating);
    }

    [Fact]
    public async Task AddItem_ExistingItem_ThrowsConflictApiException()
    {
        // Arrange
        var item = EntityGenerator.GenerateItem();
        var filter = new ItemFilterSet { ExternalId = item.ExternalId };
        await _itemsService.AddAsync(filter, item);

        // Assert
        Task Action() => _itemsService.AddAsync(filter, item);
        await Assert.ThrowsAsync<ConflictApiException>(Action);
    }

    [Fact]
    public async Task UpdateItem_ExistingItem_ItemUpdated()
    {
        // Arrange
        var item = EntityGenerator.GenerateItem();
        var filter = new ItemFilterSet { ExternalId = item.ExternalId };
        item = await _itemsService.AddAsync(filter, item);

        var payload = new Item("Updated name", "Updated Description");

        item.Patch(payload)
            .Property(x => x.Name)
            .Property(x => x.Description);

        // Act
        item = await _itemsService.UpdateAsync(filter, item);

        // Assert
        filter = new ItemFilterSet { Id = item.Id };
        var returnedItem = await _itemsService.GetAsync(filter);

        Assert.NotNull(returnedItem);
        Assert.Equal(item.Id, returnedItem.Id);
        Assert.Equal(item.ExternalId, returnedItem.ExternalId);
        Assert.Equal(item.Name, returnedItem.Name);
        Assert.Equal(item.Description, returnedItem.Description);
        Assert.NotNull(returnedItem.AverageRating);
    }

    [Fact]
    public async Task UpdateItem_NotExistingItem_AddsItem()
    {
        // Arrange
        var item = new Item("Updated name", "Updated Description");
        var filter = new ItemFilterSet { ExternalId = Guid.NewGuid() };

        // Act
        item = await _itemsService.UpdateAsync(filter, item);

        // Assert
        filter = new ItemFilterSet { Id = item.Id };
        var returnedItem = await _itemsService.GetAsync(filter);

        Assert.NotNull(returnedItem);
        Assert.Equal(item.Id, returnedItem.Id);
        Assert.Equal(item.ExternalId, returnedItem.ExternalId);
        Assert.Equal(item.Name, returnedItem.Name);
        Assert.Equal(item.Description, returnedItem.Description);
        Assert.NotNull(returnedItem.AverageRating);
    }

    [Fact]
    public async Task RemoveItem_ExistingItem_RemovesItem()
    {
        // Arrange
        var item = EntityGenerator.GenerateItem();
        var filter = new ItemFilterSet { ExternalId = item.ExternalId };
        item = await _itemsService.AddAsync(filter, item);

        // Act
        await _itemsService.RemoveAsync(filter);

        // Assert
        filter = new ItemFilterSet { Id = item.Id };
        Task Action() => _itemsService.GetAsync(filter);

        await Assert.ThrowsAsync<NotFoundApiException>(Action);
    }

    [Fact]
    public async Task RemoveItem_NotExistingItem_DoesNotThrowNotFoundApiException()
    {
        // Arrange
        var filter = new ItemFilterSet { ExternalId = Guid.NewGuid() };

        // Assert
        Task Action() => _itemsService.RemoveAsync(filter);
        var exception = await Record.ExceptionAsync(Action);
        Assert.Null(exception);
    }

    [Fact]
    public async Task GetAvailability_ExistingItem_ReturnsItemAvailabilityList()
    {
        // Arrange
        var item = EntityGenerator.GenerateItem();
        var itemFilter = new ItemFilterSet { ExternalId = item.ExternalId };
        item = await _itemsService.AddAsync(itemFilter, item);

        var locationsCount = _random.Next(1, 10);
        var itemsQuantity = _random.Next(1, 5);

        var locations = Enumerable.Range(0, locationsCount)
            .Select(EntityGenerator.GenerateLocation).ToList();

        foreach (var location in locations)
        {
            var locationFilter = new LocationFilterSet { ExternalId = location.ExternalId };
            await _locationsService.AddAsync(locationFilter, location);

            await _locationsService.UpdateStockAsync(locationFilter, itemFilter, itemsQuantity);

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
        var itemFilter = new ItemFilterSet { ExternalId = item.ExternalId };
        item = await _itemsService.AddAsync(itemFilter, item);

        var ratingCount = _random.Next(1, 10);

        var ratingUpdates = Enumerable.Range(0, ratingCount)
            .Select(async _ =>
            {
                var user = EntityGenerator.GenerateUser();
                var userFilter = new UserFilterSet { ExternalId = user.ExternalId };
                user = await _usersService.AddAsync(userFilter, user);

                var value = (byte)_random.Next(1, 5);

                return (user.ExternalId, Value: value);
            })
            .Select(x => x.Result).ToList();

        var averageRating = Math.Round((decimal)ratingUpdates.Average(x => x.Value), 2);

        itemFilter = new ItemFilterSet { Id = item.Id };

        // Act 
        foreach (var ratingUpdate in ratingUpdates)
        {
            var userFilter = new UserFilterSet { ExternalId = ratingUpdate.ExternalId };
            await _itemsService.UpdateRatingAsync(itemFilter, userFilter, ratingUpdate.Value);
        }

        // Assert
        var returnedItem = await _itemsService.GetAsync(itemFilter);

        Assert.NotNull(returnedItem);
        Assert.Equal(item.Id, returnedItem.Id);
        Assert.Equal(item.ExternalId, returnedItem.ExternalId);
        Assert.NotNull(returnedItem.AverageRating);
        Assert.Equal(averageRating, returnedItem.AverageRating);
    }

    [Fact]
    public async Task UpdateRating_NotExistingItem_ThrowsNotFoundApiException()
    {
        // Arrange
        var itemFilter = new ItemFilterSet { Id = Guid.NewGuid() };

        var user = EntityGenerator.GenerateUser();
        var userFilter = new UserFilterSet { ExternalId = user.ExternalId };
        await _usersService.AddAsync(userFilter, user);

        var value = (byte)_random.Next(1, 5);

        // Assert
        Task Action() => _itemsService.UpdateRatingAsync(itemFilter, userFilter, value);
        await Assert.ThrowsAsync<NotFoundApiException>(Action);
    }

    [Fact]
    public async Task UpdateRating_NotExistingUser_ThrowsNotFoundApiException()
    {
        // Arrange
        var item = EntityGenerator.GenerateItem();
        var itemFilter = new ItemFilterSet { ExternalId = item.ExternalId };
        item = await _itemsService.AddAsync(itemFilter, item);
        itemFilter = new ItemFilterSet { Id = item.Id };

        var userFilter = new UserFilterSet { ExternalId = Guid.NewGuid() };

        var value = (byte)_random.Next(1, 5);

        // Assert
        Task Action() => _itemsService.UpdateRatingAsync(itemFilter, userFilter, value);
        await Assert.ThrowsAsync<NotFoundApiException>(Action);
    }

    [Fact]
    public async Task RemoveRating_ExistingItem_RemovesRating()
    {
        // Arrange
        var item = EntityGenerator.GenerateItem();
        var itemFilter = new ItemFilterSet { ExternalId = item.ExternalId };
        item = await _itemsService.AddAsync(itemFilter, item);
        itemFilter = new ItemFilterSet { Id = item.Id };

        var user = EntityGenerator.GenerateUser();
        var userFilter = new UserFilterSet { ExternalId = user.ExternalId };
        user = await _usersService.AddAsync(userFilter, user);

        var value = (byte)_random.Next(1, 5);

        await _itemsService.UpdateRatingAsync(itemFilter, userFilter, value);

        // Assert
        var updatedItem = await _itemsService.RemoveRatingAsync(itemFilter, userFilter);
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
        var itemFilter = new ItemFilterSet { Id = Guid.NewGuid() };

        var user = EntityGenerator.GenerateUser();
        var userFilter = new UserFilterSet { ExternalId = user.ExternalId };
        await _usersService.AddAsync(userFilter, user);

        // Assert
        Task Action() => _itemsService.RemoveRatingAsync(itemFilter, userFilter);
        await Assert.ThrowsAsync<NotFoundApiException>(Action);
    }

    [Fact]
    public async Task RemoveRating_NotExistingUser_ThrowsNotFoundApiException()
    {
        // Arrange
        var item = EntityGenerator.GenerateItem();
        var itemFilter = new ItemFilterSet { ExternalId = item.ExternalId };
        item = await _itemsService.AddAsync(itemFilter, item);
        itemFilter = new ItemFilterSet { Id = item.Id };

        var userFilter = new UserFilterSet { ExternalId = Guid.NewGuid() };

        // Assert
        Task Action() => _itemsService.RemoveRatingAsync(itemFilter, userFilter);
        await Assert.ThrowsAsync<NotFoundApiException>(Action);
    }
}