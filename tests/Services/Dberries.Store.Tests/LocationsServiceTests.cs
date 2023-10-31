using BitzArt;
using BitzArt.ApiExceptions;
using Microsoft.Extensions.DependencyInjection;

namespace Dberries.Store.Tests;

[Collection("Service Collection")]
public class LocationsServiceTests
{
    private readonly ILocationsService _locationsService;
    private readonly IItemsService _itemsService;
    private readonly ILocationsRepository _locationsRepository;

    public LocationsServiceTests(TestServiceContainer testServiceContainer)
    {
        var serviceProvider = testServiceContainer.ServiceProvider;
        _locationsService = serviceProvider.GetRequiredService<ILocationsService>();
        _itemsService = serviceProvider.GetRequiredService<IItemsService>();
        _locationsRepository = serviceProvider.GetRequiredService<ILocationsRepository>();
    }

    [Fact]
    public async Task AddLocation_NewLocation_LocationAdded()
    {
        // Arrange
        var location = EntityGenerator.GenerateLocation();

        // Act
        location = await _locationsService.AddAsync(location);

        // Assert
        var receivedLocation = await _locationsRepository.GetAsync(location.ExternalId!.Value);

        Assert.NotNull(receivedLocation);
        Assert.Equal(location.Id, receivedLocation.Id);
        Assert.Equal(location.Name, receivedLocation.Name);
    }

    [Fact]
    public async Task AddLocation_ExistingLocation_BadRequestApiExceptionThrown()
    {
        // Arrange
        var location = EntityGenerator.GenerateLocation();
        await _locationsService.AddAsync(location);

        // Assert
        Task Action() => _locationsService.AddAsync(location);
        await Assert.ThrowsAsync<BadRequestApiException>(Action);
    }

    [Fact]
    public async Task UpdateLocation_ExistingLocation_LocationUpdated()
    {
        // Arrange
        var location = EntityGenerator.GenerateLocation();
        location = await _locationsService.AddAsync(location);

        var payload = new Location
        {
            Name = "Updated Location"
        };

        // Act
        location.Patch(payload)
            .Property(x => x.Name);

        var updatedLocation = await _locationsService.UpdateAsync(location);

        // Assert
        Assert.NotNull(updatedLocation);
        Assert.Equal(location.Id, updatedLocation.Id);
        Assert.Equal(location.ExternalId, updatedLocation.ExternalId);
        Assert.Equal(payload.Name, updatedLocation.Name);
    }

    [Fact]
    public async Task UpdateLocation_NotExistingLocation_LocationAdded()
    {
        // Arrange
        var location = new Location
        {
            ExternalId = Guid.NewGuid(),
            Name = "Updated Location"
        };

        // Act
        location = await _locationsService.UpdateAsync(location);

        // Assert
        var addedLocation = await _locationsRepository.GetAsync(location.ExternalId!.Value);

        Assert.NotNull(addedLocation);
        Assert.Equal(location.Id, addedLocation.Id);
        Assert.Equal(location.ExternalId, addedLocation.ExternalId);
        Assert.Equal(location.Name, addedLocation.Name);
    }

    [Fact]
    public async Task RemoveLocation_ExistingLocation_LocationRemoved()
    {
        // Arrange
        var location = EntityGenerator.GenerateLocation();
        location = await _locationsService.AddAsync(location);

        // Act
        await _locationsService.RemoveAsync(location.ExternalId!.Value);

        // Assert
        var deletedLocation = await _locationsRepository.GetAsync(location.ExternalId!.Value);
        Assert.Null(deletedLocation);
    }

    [Fact]
    public async Task RemoveLocation_NotExistingLocation_NotFoundApiExceptionNotThrown()
    {
        // Arrange
        var locationId = Guid.NewGuid();

        // Assert
        var exception = await Record.ExceptionAsync(() => _locationsService.RemoveAsync(locationId));
        Assert.Null(exception);
    }

    [Fact]
    public async Task UpdateStock_NewStock_StockAdded()
    {
        // Arrange
        var location = EntityGenerator.GenerateLocation();
        location = await _locationsService.AddAsync(location);

        var item = EntityGenerator.GenerateItem();
        item = await _itemsService.AddAsync(item);

        const int quantity = 10;

        // Act
        await _locationsService.UpdateStockAsync(location.ExternalId!.Value, item.ExternalId!.Value, quantity);

        // Assert 
        var itemAvailabilityList = await _itemsService.GetAvailabilityAsync(item.Id!.Value);
        var itemAvailability = itemAvailabilityList.FirstOrDefault();

        Assert.NotNull(itemAvailabilityList);
        Assert.Equal(1, itemAvailabilityList.Count);
        Assert.NotNull(itemAvailability);
        Assert.Equal(location.Id, itemAvailability.Location!.Id);
        Assert.Equal(location.ExternalId, itemAvailability.Location!.ExternalId);
        Assert.Equal(quantity, itemAvailability.Quantity);
    }

    [Fact]
    public async Task UpdateStock_ExistingStock_StockUpdated()
    {
        // Arrange
        var location = EntityGenerator.GenerateLocation();
        await _locationsService.AddAsync(location);

        var item = EntityGenerator.GenerateItem();
        await _itemsService.AddAsync(item);

        var newStock = EntityGenerator.GenerateStock();
        await _locationsService.UpdateStockAsync(location.ExternalId!.Value, item.ExternalId!.Value,
            newStock.Quantity!.Value);

        const int quantity = 10;

        // Act
        await _locationsService.UpdateStockAsync(location.ExternalId!.Value, item.ExternalId!.Value, quantity);

        // Assert
        var itemAvailabilityList = await _itemsService.GetAvailabilityAsync(item.Id!.Value);
        var itemAvailability = itemAvailabilityList.FirstOrDefault();

        Assert.NotNull(itemAvailabilityList);
        Assert.Equal(1, itemAvailabilityList.Count);
        Assert.NotNull(itemAvailability);
        Assert.Equal(location.Id, itemAvailability.Location!.Id);
        Assert.Equal(location.ExternalId, itemAvailability.Location!.ExternalId);
        Assert.Equal(quantity, itemAvailability.Quantity);
    }

    [Fact]
    public async Task UpdateStock_ZeroQuantity_StockRemoved()
    {
        // Arrange
        var location = EntityGenerator.GenerateLocation();
        location = await _locationsService.AddAsync(location);

        var item = EntityGenerator.GenerateItem();
        item = await _itemsService.AddAsync(item);

        var stock = EntityGenerator.GenerateStock();
        await _locationsService.UpdateStockAsync(location.ExternalId!.Value, item.ExternalId!.Value,
            stock.Quantity!.Value);

        const int quantity = 0;

        // Act
        await _locationsService.UpdateStockAsync(location.ExternalId!.Value, item.ExternalId!.Value, quantity);

        // Assert
        var itemAvailabilityList = await _itemsService.GetAvailabilityAsync(item.Id!.Value);

        Assert.NotNull(itemAvailabilityList);
        Assert.Equal(quantity, itemAvailabilityList.Count);
    }

    [Fact]
    public async Task UpdateStock_NotExistingLocation_NotFoundApiExceptionThrown()
    {
        // Arrange
        var item = EntityGenerator.GenerateItem();
        item = await _itemsService.AddAsync(item);

        var locationId = Guid.NewGuid();
        const int quantity = 10;

        // Assert
        Task Action() => _locationsService.UpdateStockAsync(locationId, item.ExternalId!.Value, quantity);
        await Assert.ThrowsAsync<NotFoundApiException>(Action);
    }

    [Fact]
    public async Task UpdateStock_NotExistingItem_NotFoundApiExceptionThrown()
    {
        // Arrange
        var location = EntityGenerator.GenerateLocation();
        await _locationsService.AddAsync(location);

        var itemId = Guid.NewGuid();
        const int quantity = 10;

        // Assert
        Task Action() => _locationsService.UpdateStockAsync(location.Id!.Value, itemId, quantity);
        await Assert.ThrowsAsync<NotFoundApiException>(Action);
    }

    [Fact]
    public async Task UpdateStock_NegativeQuantity_BadRequestApiExceptionThrown()
    {
        // Arrange
        var location = EntityGenerator.GenerateLocation();
        await _locationsService.AddAsync(location);

        var item = EntityGenerator.GenerateItem();
        await _itemsService.AddAsync(item);

        var existingStock = EntityGenerator.GenerateStock();
        await _locationsService.UpdateStockAsync(location.ExternalId!.Value, item.ExternalId!.Value,
            existingStock.Quantity!.Value);

        const int quantity = -10;

        // Assert
        Task Action() =>
            _locationsService.UpdateStockAsync(location.ExternalId!.Value, item.ExternalId!.Value, quantity);

        await Assert.ThrowsAsync<BadRequestApiException>(Action);
    }
}