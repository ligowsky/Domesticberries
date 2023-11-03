using BitzArt;
using BitzArt.ApiExceptions;
using BitzArt.Pagination;
using Microsoft.Extensions.DependencyInjection;

namespace Dberries.Warehouse.Tests;

[Collection("Service Collection")]
public class LocationsServiceTests
{
    private readonly ILocationsService _locationsService;
    private readonly IItemsService _itemsService;
    private readonly AppDbContext _db;

    public LocationsServiceTests(TestServiceContainer testServiceContainer)
    {
        var serviceProvider = testServiceContainer.ServiceProvider;
        _locationsService = serviceProvider.GetRequiredService<ILocationsService>();
        _itemsService = serviceProvider.GetRequiredService<IItemsService>();
        _db = serviceProvider.GetRequiredService<AppDbContext>();
    }

    [Fact]
    public async Task GetLocationsPage_ExistingLocations_ReturnsLocationsPage()
    {
        // Arrange
        const int locationsCount = 10;

        for (var i = 0; i < locationsCount; i++)
        {
            var location = EntityGenerator.GenerateLocation();
            await _locationsService.AddAsync(location);
        }

        var pageRequest = new PageRequest(0, locationsCount);

        // Act
        var locationsPage = await _locationsService.GetPageAsync(pageRequest);

        // Assert
        Assert.NotNull(locationsPage);
        Assert.NotNull(locationsPage.Data);
        Assert.Equal(locationsCount, locationsPage.Data.Count());
    }

    [Fact]
    public async Task GetLocation_ExistingLocation_ReturnsLocation()
    {
        // Arrange
        var location = EntityGenerator.GenerateLocation();
        await _locationsService.AddAsync(location);

        // Assert
        var returnedLocation = await _locationsService.GetAsync(location.Id!.Value);

        Assert.NotNull(returnedLocation);
        Assert.Equal(location.Id, returnedLocation.Id);
        Assert.Equal(location.Name, returnedLocation.Name);
        Assert.Equal(location.Coordinates, returnedLocation.Coordinates);
    }

    [Fact]
    public async Task GetLocation_NotExistingLocation_ThrowsNotFoundApiException()
    {
        // Arrange
        var locationId = Guid.NewGuid();

        // Assert
        Task Action() => _locationsService.GetAsync(locationId);
        await Assert.ThrowsAsync<NotFoundApiException>(Action);
    }

    [Fact]
    public async Task AddLocation_NewLocation_AddsLocation()
    {
        // Arrange
        var location = EntityGenerator.GenerateLocation();

        // Act
        await _locationsService.AddAsync(location);

        // Assert
        var addedLocation = await _locationsService.GetAsync(location.Id!.Value);

        Assert.NotNull(addedLocation);
        Assert.Equal(location.Name, addedLocation.Name);
        Assert.NotNull(addedLocation.Coordinates);
        Assert.Equal(location.Coordinates!.Latitude, addedLocation.Coordinates.Latitude);
        Assert.Equal(location.Coordinates!.Longitude, addedLocation.Coordinates.Longitude);
    }

    [Fact]
    public async Task UpdateLocation_ExistingLocation_UpdatesLocation()
    {
        // Arrange
        var location = EntityGenerator.GenerateLocation();
        location = await _locationsService.AddAsync(location);

        var payload = new Location("Updated Location", new Coordinates(10, 20));

        location.Patch(payload)
            .Property(x => x.Name)
            .Property(x => x.Coordinates);

        // Act
        await _locationsService.UpdateAsync(location.Id!.Value, location);

        // Assert
        var updatedLocation = await _locationsService.GetAsync(location.Id!.Value);

        Assert.NotNull(updatedLocation);
        Assert.Equal(location.Id, updatedLocation.Id);
        Assert.Equal(payload.Name, updatedLocation.Name);
        Assert.NotNull(updatedLocation.Coordinates);
        Assert.Equal(payload.Coordinates!.Latitude, updatedLocation.Coordinates.Latitude);
        Assert.Equal(payload.Coordinates!.Longitude, updatedLocation.Coordinates!.Longitude);
    }

    [Fact]
    public async Task UpdateLocation_NotExistingLocation_ThrowsNotFoundApiException()
    {
        // Arrange
        var locationId = Guid.NewGuid();
        var location = EntityGenerator.GenerateLocation();

        // Assert
        Task Action() => _locationsService.UpdateAsync(locationId, location);
        await Assert.ThrowsAsync<NotFoundApiException>(Action);
    }

    [Fact]
    public async Task RemoveLocation_ExistingLocation_RemovesLocation()
    {
        // Arrange
        var location = EntityGenerator.GenerateLocation();
        location = await _locationsService.AddAsync(location);

        // Act
        await _locationsService.RemoveAsync(location.Id!.Value);

        // Assert
        Task Action() => _locationsService.GetAsync(location.Id!.Value);
        await Assert.ThrowsAsync<NotFoundApiException>(Action);
    }

    [Fact]
    public async Task RemoveLocation_NotExistingLocation_ThrowsNotFoundApiException()
    {
        // Arrange
        var locationId = Guid.NewGuid();

        // Assert
        Task Action() => _locationsService.RemoveAsync(locationId);
        await Assert.ThrowsAsync<NotFoundApiException>(Action);
    }

    [Fact]
    public async Task GetStockPage_ExistingStock_ReturnsStockPage()
    {
        // Arrange
        var location = EntityGenerator.GenerateLocation();
        location = await _locationsService.AddAsync(location);

        const int count = 10;

        var items = Enumerable.Range(0, count)
            .Select(EntityGenerator.GenerateItem);

        foreach (var item in items)
        {
            await _itemsService.AddAsync(item);

            var stock = EntityGenerator.GenerateStock();
            await _locationsService.UpdateStockAsync(location.Id!.Value, item.Id!.Value, stock);

            _db.ChangeTracker.Clear();
        }

        var pageRequest = new PageRequest(0, count);

        // Act
        var stockPage = await _locationsService.GetStockPageAsync(location.Id!.Value, pageRequest);

        // Assert
        Assert.NotNull(stockPage);
        Assert.NotNull(stockPage.Data);
        Assert.Equal(count, stockPage.Data.Count());
    }

    [Theory]
    [InlineData(1)]
    [InlineData(10)]
    [InlineData(100)]
    public async Task UpdateStock_NewStock_AddsStock(int quantity)
    {
        // Arrange
        var location = EntityGenerator.GenerateLocation();
        await _locationsService.AddAsync(location);

        var item = EntityGenerator.GenerateItem();
        await _itemsService.AddAsync(item);

        var stock = EntityGenerator.GenerateStock(quantity);

        // Act
        await _locationsService.UpdateStockAsync(location.Id!.Value, item.Id!.Value, stock);

        // Assert
        var addedStock = await _locationsService.GetStockAsync(location.Id!.Value, item.Id!.Value);

        Assert.NotNull(addedStock);
        Assert.Equal(quantity, addedStock.Quantity);
        Assert.Equal(item.Id, addedStock.ItemId);
    }

    [Theory]
    [InlineData(10)]
    [InlineData(100)]
    [InlineData(1000)]
    public async Task UpdateStock_ExistingStock_UpdatesStock(int quantity)
    {
        // Arrange
        var location = EntityGenerator.GenerateLocation();
        await _locationsService.AddAsync(location);

        var item = EntityGenerator.GenerateItem();
        await _itemsService.AddAsync(item);

        var newStock = EntityGenerator.GenerateStock();
        var existingStock = await _locationsService.UpdateStockAsync(location.Id!.Value, item.Id!.Value, newStock);

        var stock = EntityGenerator.GenerateStock(quantity);

        // Act
        await _locationsService.UpdateStockAsync(location.Id!.Value, item.Id!.Value, stock);

        // Assert
        var updatedStock = await _locationsService.GetStockAsync(location.Id!.Value, item.Id!.Value);

        Assert.NotNull(existingStock);
        Assert.NotNull(updatedStock);
        Assert.Equal(quantity, updatedStock.Quantity);
        Assert.Equal(item.Id, updatedStock.ItemId);
        Assert.Equal(existingStock.ItemId, updatedStock.ItemId);
    }

    [Fact]
    public async Task UpdateStock_ZeroQuantity_RemovesStock()
    {
        // Arrange
        var location = EntityGenerator.GenerateLocation();
        location = await _locationsService.AddAsync(location);

        var item = EntityGenerator.GenerateItem();
        item = await _itemsService.AddAsync(item);

        var existingStock = EntityGenerator.GenerateStock();
        await _locationsService.UpdateStockAsync(location.Id!.Value, item.Id!.Value, existingStock);

        var stock = EntityGenerator.GenerateStock(0);

        // Act
        var updatedStock = await _locationsService.UpdateStockAsync(location.Id!.Value, item.Id!.Value, stock);

        // Assert
        Assert.NotNull(existingStock);
        Assert.NotNull(updatedStock);
        Assert.Equal(0, updatedStock.Quantity);
    }

    [Fact]
    public async Task UpdateStock_NotExistingLocation_ThrowsNotFoundApiException()
    {
        // Arrange
        var item = EntityGenerator.GenerateItem();
        item = await _itemsService.AddAsync(item);

        var locationId = Guid.NewGuid();
        var stock = EntityGenerator.GenerateStock();

        // Assert
        Task Action() => _locationsService.UpdateStockAsync(locationId, item.Id!.Value, stock);
        await Assert.ThrowsAsync<NotFoundApiException>(Action);
    }

    [Fact]
    public async Task UpdateStock_NotExistingItem_ThrowsNotFoundApiException()
    {
        // Arrange
        var location = EntityGenerator.GenerateLocation();
        await _locationsService.AddAsync(location);

        var itemId = Guid.NewGuid();
        var stock = EntityGenerator.GenerateStock();

        // Assert
        Task Action() => _locationsService.UpdateStockAsync(location.Id!.Value, itemId, stock);
        await Assert.ThrowsAsync<NotFoundApiException>(Action);
    }

    [Fact]
    public async Task UpdateStock_NegativeQuantity_ThrowsBadRequestApiException()
    {
        // Arrange
        var location = EntityGenerator.GenerateLocation();
        await _locationsService.AddAsync(location);

        var item = EntityGenerator.GenerateItem();
        await _itemsService.AddAsync(item);

        var existingStock = EntityGenerator.GenerateStock();
        await _locationsService.UpdateStockAsync(location.Id!.Value, item.Id!.Value, existingStock);

        const int quantity = -1;
        var stock = EntityGenerator.GenerateStock(quantity);

        // Assert
        Task Action() => _locationsService.UpdateStockAsync(location.Id!.Value, item.Id!.Value, stock);
        await Assert.ThrowsAsync<BadRequestApiException>(Action);
    }
}