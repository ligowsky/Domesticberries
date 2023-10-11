using BitzArt;
using BitzArt.ApiExceptions;
using BitzArt.Pagination;
using Dberries.Warehouse.Persistence;
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
    public async Task GetLocationsPage_ExistingLocations_LocationsReceived()
    {
        // Arrange
        const int locationsCount = 10;

        for (var i = 0; i < locationsCount; i++)
        {
            var location = GenerateLocation();
            await _locationsService.AddAsync(location);
        }

        var pageRequest = new PageRequest()
        {
            Offset = 0,
            Limit = locationsCount
        };

        // Act
        var locationsPage = await _locationsService.GetPageAsync(pageRequest);

        // Assert
        Assert.NotNull(locationsPage);
        Assert.Equal(locationsCount, locationsPage.Data!.Count());
    }

    [Fact]
    public async Task GetLocation_ExistingLocation_LocationReceived()
    {
        // Arrange
        var location = GenerateLocation();
        await _locationsService.AddAsync(location);

        // Act
        var existingLocation = await _locationsService.GetAsync(location.Id!.Value);

        // Assert
        Assert.NotNull(existingLocation);
        Assert.Equal(location.Id, existingLocation.Id);
        Assert.Equal(location.Name, existingLocation.Name);
        Assert.Equal(location.Coordinates, existingLocation.Coordinates);
    }

    [Fact]
    public async Task GetLocation_NotExistingLocation_NotFoundApiExceptionThrown()
    {
        // Arrange
        var locationId = Guid.NewGuid();
        var exceptionType = typeof(NotFoundApiException);
        var expectedMessage = $"{nameof(Location)} with id '{locationId}' is not found";

        // Act
        var exception =
            await Record.ExceptionAsync(async () => await _locationsService.GetAsync(locationId));

        // Assert
        Assert.IsType(exceptionType, exception);
        Assert.Equal(expectedMessage, exception.Message);
    }

    [Fact]
    public async Task AddLocation_NewLocation_LocationAdded()
    {
        // Arrange
        var location = GenerateLocation();

        // Act
        await _locationsService.AddAsync(location);

        // Assert
        var createdLocation = await _locationsService.GetAsync(location.Id!.Value);

        Assert.NotNull(createdLocation);
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
        var newLocation = GenerateLocation();
        await _locationsService.AddAsync(newLocation);
        var existingLocation = await _locationsService.GetAsync(newLocation.Id!.Value);

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
        existingLocation.Patch(location)
            .Property(x => x.Name)
            .Property(x => x.Coordinates);

        await _locationsService.UpdateAsync(existingLocation.Id!.Value, existingLocation);

        // Assert
        var updatedLocation = await _locationsService.GetAsync(existingLocation.Id!.Value);

        Assert.NotNull(updatedLocation);
        Assert.Equal(existingLocation.Id, updatedLocation.Id);
        Assert.Equal(updatedLocation.Name, name);
        Assert.NotNull(updatedLocation.Coordinates);
        Assert.Equal(updatedLocation.Coordinates!.Latitude, latitude);
        Assert.Equal(updatedLocation.Coordinates!.Longitude, longitude);
    }

    [Fact]
    public void UpdateLocation_NotExistingLocation_NotFoundApiExceptionThrown()
    {
        // Arrange
        var locationId = Guid.NewGuid();
        var location = GenerateLocation();
        var exceptionType = typeof(NotFoundApiException);
        var expectedMessage = $"{nameof(Location)} with id '{locationId}' is not found";

        // Act
        var exception =
            Record.ExceptionAsync(async () => await _locationsService.UpdateAsync(locationId, location));

        // Assert
        Assert.IsType(exceptionType, exception.Result);
        Assert.Equal(expectedMessage, exception.Result.Message);
    }

    [Fact]
    public async Task RemoveLocation_ExistingLocation_LocationRemoved()
    {
        // Arrange
        var newLocation = GenerateLocation();
        await _locationsService.AddAsync(newLocation);
        var existingLocation = await _locationsService.GetAsync(newLocation.Id!.Value);
        var exceptionType = typeof(NotFoundApiException);
        var expectedMessage = $"{nameof(Location)} with id '{existingLocation.Id}' is not found";

        // Act
        await _locationsService.RemoveAsync(existingLocation.Id!.Value);

        // Assert
        var exception =
            await Record.ExceptionAsync(
                async () => await _locationsService.GetAsync(existingLocation.Id!.Value));

        Assert.IsType(exceptionType, exception);
        Assert.Equal(expectedMessage, exception.Message);
    }

    [Fact]
    public async Task RemoveLocation_NotExistingLocation_NotFoundApiExceptionThrown()
    {
        // Arrange
        var locationId = Guid.NewGuid();
        var exceptionType = typeof(NotFoundApiException);
        var expectedMessage = $"{nameof(Location)} with id '{locationId}' is not found";

        // Act
        var exception =
            await Record.ExceptionAsync(async () => await _locationsService.RemoveAsync(locationId));

        // Assert
        Assert.IsType(exceptionType, exception);
        Assert.Equal(expectedMessage, exception.Message);
    }

    [Fact]
    public async Task GetStockPage_ExistingStock_StockPageReceived()
    {
        // Arrange
        var location = GenerateLocation();
        var createdLocation = await _locationsService.AddAsync(location);

        const int count = 10;

        var items = Enumerable.Range(0, count).Select(GenerateItem);

        foreach (var item in items)
        {
            await _itemsService.AddAsync(item);

            var stock = GenerateStock();
            await _locationsService.UpdateStockAsync(location.Id!.Value, item.Id!.Value, stock);

            _db.ChangeTracker.Clear();
        }

        var pageRequest = new PageRequest
        {
            Offset = 0,
            Limit = count
        };

        // Act
        var stockPage = await _locationsService.GetStockPageAsync(createdLocation.Id!.Value, pageRequest);

        // Assert
        Assert.NotNull(stockPage);
        Assert.Equal(count, stockPage.Data!.Count());
    }

    [Theory]
    [InlineData(1)]
    [InlineData(10)]
    [InlineData(100)]
    public async Task UpdateStock_NewStock_StockAdded(int quantity)
    {
        // Arrange
        var location = GenerateLocation();
        await _locationsService.AddAsync(location);

        var item = GenerateItem();
        await _itemsService.AddAsync(item);

        var stock = GenerateStock(quantity);

        // Act
        await _locationsService.UpdateStockAsync(location.Id!.Value, item.Id!.Value, stock);

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
        var location = GenerateLocation();
        await _locationsService.AddAsync(location);

        var item = GenerateItem();
        await _itemsService.AddAsync(item);

        var existingStock = GenerateStock();

        var stock = GenerateStock(quantity);

        existingStock = await _locationsService.UpdateStockAsync(location.Id!.Value, item.Id!.Value, existingStock);

        // Act
        await _locationsService.UpdateStockAsync(location.Id!.Value, item.Id!.Value, stock);

        // Assert
        var updatedStock = await _locationsService.GetStockAsync(location.Id!.Value, item.Id!.Value);

        Assert.NotNull(existingStock);
        Assert.NotNull(updatedStock);
        Assert.Equal(updatedStock.Quantity, quantity);
        Assert.Equal(updatedStock.ItemId, item.Id);
        Assert.Equal(existingStock.ItemId, updatedStock.ItemId);
    }

    [Fact]
    public async Task UpdateStock_ZeroQuantity_StockRemoved()
    {
        // Arrange
        var location = GenerateLocation();
        await _locationsService.AddAsync(location);

        var item = GenerateItem();
        await _itemsService.AddAsync(item);

        var existingStock = GenerateStock();
        existingStock = await _locationsService.UpdateStockAsync(location.Id!.Value, item.Id!.Value, existingStock);

        var stock = GenerateStock(0);

        // Act
        await _locationsService.UpdateStockAsync(location.Id!.Value, item.Id!.Value, stock);

        // Assert
        var updatedStock = await _locationsService.GetStockAsync(location.Id!.Value, item.Id!.Value);

        Assert.NotNull(existingStock);
        Assert.Null(updatedStock);
    }

    [Fact]
    public async Task UpdateStock_NotExistingLocation_NotFoundApiExceptionThrown()
    {
        // Arrange
        var location = GenerateLocation();
        await _locationsService.AddAsync(location);

        var item = GenerateItem();
        await _itemsService.AddAsync(item);

        var existingStock = GenerateStock();
        await _locationsService.UpdateStockAsync(location.Id!.Value, item.Id!.Value, existingStock);

        var stock = GenerateStock();
        var locationId = Guid.NewGuid();
        var exceptionType = typeof(NotFoundApiException);
        var expectedMessage = $"{nameof(Location)} with id '{locationId}' is not found";

        // Act
        var exception =
            await Record.ExceptionAsync(async () =>
                await _locationsService.UpdateStockAsync(locationId, item.Id!.Value, stock));

        // Assert
        Assert.IsType(exceptionType, exception);
        Assert.Equal(expectedMessage, exception.Message);
    }

    [Fact]
    public async Task UpdateStock_NotExistingItem_NotFoundApiExceptionThrown()
    {
        // Arrange
        var location = GenerateLocation();
        await _locationsService.AddAsync(location);

        var item = GenerateItem();
        await _itemsService.AddAsync(item);

        var existingStock = GenerateStock();
        await _locationsService.UpdateStockAsync(location.Id!.Value, item.Id!.Value, existingStock);

        var stock = GenerateStock();
        var itemId = Guid.NewGuid();
        var exceptionType = typeof(NotFoundApiException);
        var expectedMessage = $"{nameof(Item)} with id '{itemId}' is not found";

        // Act
        var exception =
            await Record.ExceptionAsync(async () =>
                await _locationsService.UpdateStockAsync(location.Id!.Value, itemId, stock));

        // Assert
        Assert.IsType(exceptionType, exception);
        Assert.Equal(expectedMessage, exception.Message);
    }

    [Theory]
    [InlineData(-1)]
    [InlineData(-100)]
    [InlineData(-1000)]
    public async Task UpdateStock_NegativeQuantity_BadRequestApiExceptionThrown(int quantity)
    {
        // Arrange
        var location = GenerateLocation();
        await _locationsService.AddAsync(location);

        var item = GenerateItem();
        await _itemsService.AddAsync(item);

        var existingStock = GenerateStock();
        await _locationsService.UpdateStockAsync(location.Id!.Value, item.Id!.Value, existingStock);

        var stock = GenerateStock(quantity);
        var exceptionType = typeof(BadRequestApiException);
        var expectedMessage = ($"Invalid quantity: {quantity}. Quantity must be greater than 0.");

        // Act
        var exception =
            await Record.ExceptionAsync(async () =>
                await _locationsService.UpdateStockAsync(location.Id!.Value, item.Id!.Value, stock));

        // Assert
        Assert.IsType(exceptionType, exception);
        Assert.Equal(expectedMessage, exception.Message);
    }

    private Location GenerateLocation(int number = 1)
    {
        return new Location
        {
            Name = $"Location {number}",
            Coordinates = new Coordinates
            {
                Latitude = 1,
                Longitude = 1
            }
        };
    }

    private Item GenerateItem(int number = 1)
    {
        return new Item
        {
            Name = $"Item {number}",
            Description = $"Description {number}"
        };
    }

    public Stock GenerateStock(int quantity = 1)
    {
        return new Stock
        {
            Quantity = quantity
        };
    }
}