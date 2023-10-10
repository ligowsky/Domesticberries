using BitzArt.ApiExceptions;
using BitzArt.Pagination;
using Microsoft.Extensions.DependencyInjection;
using Xunit.Abstractions;

namespace Dberries.Warehouse.Tests;

[Collection("Service Collection")]
public class LocationsServiceTests
{
    private readonly ITestOutputHelper _testOutputHelper;
    private readonly ILocationsService _locationsService;
    private readonly IItemsService _itemsService;

    public LocationsServiceTests(TestServiceContainer testServiceContainer, ITestOutputHelper testOutputHelper)
    {
        _testOutputHelper = testOutputHelper;
        _locationsService = testServiceContainer.ServiceProvider.GetRequiredService<ILocationsService>();
        _itemsService = testServiceContainer.ServiceProvider.GetRequiredService<IItemsService>();
    }

    [Fact]
    public async Task GetLocationsPage_ExistingLocations_LocationsReceived()
    {
        // Arrange
        const int locationsCount = 10;

        for (var i = 0; i < locationsCount; i++)
        {
            var location = InitNewLocation();
            await _locationsService.CreateLocationAsync(location);
        }

        var pageRequest = new PageRequest()
        {
            Offset = 0,
            Limit = locationsCount
        };

        // Act
        var locationsPage = await _locationsService.GetLocationsPageAsync(pageRequest);

        // Assert
        Assert.NotNull(locationsPage);
        Assert.Equal(locationsCount, locationsPage.Data!.Count());
    }

    [Fact]
    public async Task GetLocation_ExistingLocation_LocationReceived()
    {
        // Arrange
        var location = InitNewLocation();
        await _locationsService.CreateLocationAsync(location);

        // Act
        var existingLocation = await _locationsService.GetLocationAsync(location.Id!.Value);

        // Assert
        Assert.NotNull(existingLocation);
        Assert.Equal(location.Id, existingLocation.Id);
        Assert.Equal(location.Name, existingLocation.Name);
        Assert.Equal(location.Coordinates, existingLocation.Coordinates);
    }

    [Fact]
    public async Task GetLocation_NotExistingLocation_LocationNotFound()
    {
        // Arrange
        var locationId = Guid.NewGuid();
        var exceptionType = typeof(NotFoundApiException);
        var expectedMessage = $"{nameof(Location)} with id '{locationId}' is not found";

        // Act
        var exception =
            await Record.ExceptionAsync(async () => await _locationsService.GetLocationAsync(locationId));

        // Assert
        Assert.IsType(exceptionType, exception);
        Assert.Equal(expectedMessage, exception.Message);
    }

    [Fact]
    public async Task CreateLocation_NewLocation_LocationCreated()
    {
        // Arrange
        var location = InitNewLocation();

        // Act
        await _locationsService.CreateLocationAsync(location);

        // Assert
        var createdLocation = await _locationsService.GetLocationAsync(location.Id!.Value);

        Assert.NotNull(createdLocation);
        Assert.Equal(location.Id, createdLocation.Id);
        Assert.Equal(location.Name, createdLocation.Name);
        Assert.Equal(location.Coordinates, createdLocation.Coordinates);
    }

    [Theory]
    [InlineData("Location 2", 40.7128, -74.0060)]
    [InlineData("Location 3", -33.8688, 151.2093)]
    [InlineData("Location 4", 51.5074, -0.1278)]
    public async Task UpdateLocation_ExistingLocation_LocationUpdated(string name, double latitude, double longitude)
    {
        // Arrange
        var newLocation = InitNewLocation();
        await _locationsService.CreateLocationAsync(newLocation);
        var existingLocation = await _locationsService.GetLocationAsync(newLocation.Id!.Value);

        var location = new Location
        {
            Name = name,
            Coordinates = new Coordinates
            {
                Latitude = latitude,
                Longitude = longitude
            }
        };

        // Act
        existingLocation.Update(location);
        await _locationsService.UpdateLocationAsync(existingLocation.Id!.Value, existingLocation);

        // Assert
        var updatedLocation = await _locationsService.GetLocationAsync(existingLocation.Id!.Value);

        Assert.NotNull(updatedLocation);
        Assert.Equal(existingLocation.Id, updatedLocation.Id);
        Assert.Equal(updatedLocation.Name, name);
        Assert.NotNull(updatedLocation.Coordinates);
        Assert.Equal(updatedLocation.Coordinates!.Latitude, latitude);
        Assert.Equal(updatedLocation.Coordinates!.Longitude, longitude);
    }

    [Fact]
    public void UpdateLocation_NotExistingLocation_LocationNotFound()
    {
        // Arrange
        var locationId = Guid.NewGuid();
        var location = InitNewLocation();
        var exceptionType = typeof(NotFoundApiException);
        var expectedMessage = $"{nameof(Location)} with id '{locationId}' is not found";

        // Act
        var exception =
            Record.ExceptionAsync(async () => await _locationsService.UpdateLocationAsync(locationId, location));

        // Assert
        Assert.IsType(exceptionType, exception.Result);
        Assert.Equal(expectedMessage, exception.Result.Message);
    }

    [Fact]
    public async Task DeleteLocation_ExistingLocation_LocationDeleted()
    {
        // Arrange
        var newLocation = InitNewLocation();
        await _locationsService.CreateLocationAsync(newLocation);
        var existingLocation = await _locationsService.GetLocationAsync(newLocation.Id!.Value);
        var exceptionType = typeof(NotFoundApiException);
        var expectedMessage = $"{nameof(Location)} with id '{existingLocation.Id}' is not found";

        // Act
        await _locationsService.DeleteLocationAsync(existingLocation.Id!.Value);

        // Assert
        var exception =
            await Record.ExceptionAsync(
                async () => await _locationsService.GetLocationAsync(existingLocation.Id!.Value));

        Assert.IsType(exceptionType, exception);
        Assert.Equal(expectedMessage, exception.Message);
    }

    [Fact]
    public async Task DeleteLocation_NotExistingLocation_LocationNotFound()
    {
        // Arrange
        var locationId = Guid.NewGuid();
        var exceptionType = typeof(NotFoundApiException);
        var expectedMessage = $"{nameof(Location)} with id '{locationId}' is not found";

        // Act
        var exception =
            await Record.ExceptionAsync(async () => await _locationsService.DeleteLocationAsync(locationId));

        // Assert
        Assert.IsType(exceptionType, exception);
        Assert.Equal(expectedMessage, exception.Message);
    }

    [Fact]
    public async Task GetStockPage_ExistingStock_StockReceived()
    {
        // Arrange
        var location = InitNewLocation();
        var createdLocation = await _locationsService.CreateLocationAsync(location);

        const int stockCount = 10;
        const int quantity = 5;

        for (var i = 0; i < stockCount; i++)
        {
            var item = InitNewItem();
            var createdItem = await _itemsService.CreateItemAsync(item);

            var stock = await _locationsService.UpdateStockAsync(createdLocation.Id!.Value, createdItem.Id!.Value, quantity);
            _testOutputHelper.WriteLine(stock?.ToString());
        }

        var pageRequest = new PageRequest()
        {
            Offset = 0,
            Limit = stockCount
        };

        // Act
        var stockPage = await _locationsService.GetStockPageAsync(createdLocation.Id!.Value, pageRequest);

        // Assert
        Assert.NotNull(stockPage);
        Assert.Equal(stockCount, stockPage.Data!.Count());
    }

    [Theory]
    [InlineData(1)]
    [InlineData(10)]
    [InlineData(100)]
    public async Task UpdateStock_NewStock_StockAdded(int quantity)
    {
        // Arrange
        var location = InitNewLocation();
        await _locationsService.CreateLocationAsync(location);

        var item = InitNewItem();
        await _itemsService.CreateItemAsync(item);

        // Act
        await _locationsService.UpdateStockAsync(location.Id!.Value, item.Id!.Value, quantity);

        // Assert
        var createdStock = await _locationsService.GetStockAsync(location.Id!.Value, item.Id!.Value);

        Assert.NotNull(createdStock);
        Assert.Equal(createdStock.Quantity, quantity);
        Assert.Equal(createdStock.ItemId, item.Id);
    }

    [Theory]
    [InlineData(10)]
    [InlineData(100)]
    [InlineData(1000)]
    public async Task UpdateStock_ExistingStock_StockUpdated(int quantity)
    {
        // Arrange
        var location = InitNewLocation();
        await _locationsService.CreateLocationAsync(location);

        var item = InitNewItem();
        await _itemsService.CreateItemAsync(item);

        var existingStock = await _locationsService.UpdateStockAsync(location.Id!.Value, item.Id!.Value, 1);

        // Act
        await _locationsService.UpdateStockAsync(location.Id!.Value, item.Id!.Value, quantity);

        // Assert
        var updatedStock = await _locationsService.GetStockAsync(location.Id!.Value, item.Id!.Value);

        Assert.NotNull(existingStock);
        Assert.NotNull(updatedStock);
        Assert.Equal(updatedStock.Quantity, quantity);
        Assert.Equal(updatedStock.ItemId, item.Id);
        Assert.Equal(existingStock.ItemId, updatedStock.ItemId);
    }

    [Fact]
    public async Task UpdateStock_ExistingStock_StockDeleted()
    {
        // Arrange
        var location = InitNewLocation();
        await _locationsService.CreateLocationAsync(location);

        var item = InitNewItem();
        await _itemsService.CreateItemAsync(item);

        var existingStock = await _locationsService.UpdateStockAsync(location.Id!.Value, item.Id!.Value, 1);

        // Act
        await _locationsService.UpdateStockAsync(location.Id!.Value, item.Id!.Value, 0);

        // Assert
        var updatedStock = await _locationsService.GetStockAsync(location.Id!.Value, item.Id!.Value);

        Assert.NotNull(existingStock);
        Assert.Null(updatedStock);
    }

    [Fact]
    public async Task UpdateStock_ExistingStock_LocationNotFound()
    {
        // Arrange
        var location = InitNewLocation();
        await _locationsService.CreateLocationAsync(location);

        var item = InitNewItem();
        await _itemsService.CreateItemAsync(item);

        await _locationsService.UpdateStockAsync(location.Id!.Value, item.Id!.Value, 1);

        const int quantity = 10;
        var locationId = Guid.NewGuid();
        var exceptionType = typeof(NotFoundApiException);
        var expectedMessage = $"{nameof(Location)} with id '{locationId}' is not found";

        // Act
        var exception =
            await Record.ExceptionAsync(async () =>
                await _locationsService.UpdateStockAsync(locationId, item.Id!.Value, quantity));

        // Assert
        Assert.IsType(exceptionType, exception);
        Assert.Equal(expectedMessage, exception.Message);
    }

    [Fact]
    public async Task UpdateStock_ExistingStock_ItemNotFound()
    {
        // Arrange
        var location = InitNewLocation();
        await _locationsService.CreateLocationAsync(location);

        var item = InitNewItem();
        await _itemsService.CreateItemAsync(item);

        await _locationsService.UpdateStockAsync(location.Id!.Value, item.Id!.Value, 1);

        const int quantity = 10;
        var itemId = Guid.NewGuid();
        var exceptionType = typeof(NotFoundApiException);
        var expectedMessage = $"{nameof(Item)} with id '{itemId}' is not found";

        // Act
        var exception =
            await Record.ExceptionAsync(async () =>
                await _locationsService.UpdateStockAsync(location.Id!.Value, itemId, quantity));

        // Assert
        Assert.IsType(exceptionType, exception);
        Assert.Equal(expectedMessage, exception.Message);
    }

    private Location InitNewLocation()
    {
        return new Location
        {
            Name = "Location 1",
            Coordinates = new Coordinates
            {
                Latitude = 1,
                Longitude = 1
            }
        };
    }

    private Item InitNewItem()
    {
        return new Item
        {
            Name = "Item 1",
            Description = "Description 1"
        };
    }
}