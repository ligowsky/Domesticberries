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
    private readonly Random _random;

    public LocationsServiceTests(TestServiceContainer testServiceContainer)
    {
        var serviceProvider = testServiceContainer.ServiceProvider;
        _locationsService = serviceProvider.GetRequiredService<ILocationsService>();
        _itemsService = serviceProvider.GetRequiredService<IItemsService>();
        _locationsRepository = serviceProvider.GetRequiredService<ILocationsRepository>();
        _random = new Random();
    }

    [Fact]
    public async Task AddLocation_NewLocation_AddsLocation()
    {
        // Arrange
        var location = EntityGenerator.GenerateLocation();

        // Act
        location = await _locationsService.AddAsync(location);

        // Assert
        var filter = new LocationFilterSet { Id = location.Id };
        var addedLocation = await _locationsRepository.GetAsync(filter);

        Assert.NotNull(addedLocation);
        Assert.Equal(location.Id, addedLocation.Id);
        Assert.Equal(location.ExternalId, addedLocation.ExternalId);
        Assert.Equal(location.Name, addedLocation.Name);
    }

    [Fact]
    public async Task AddLocation_ExistingLocation_ThrowsConflictApiException()
    {
        // Arrange
        var location = EntityGenerator.GenerateLocation();
        await _locationsService.AddAsync(location);

        // Assert
        Task Action() => _locationsService.AddAsync(location);
        await Assert.ThrowsAsync<ConflictApiException>(Action);
    }

    [Fact]
    public async Task UpdateLocation_ExistingLocation_UpdatesLocation()
    {
        // Arrange
        var location = EntityGenerator.GenerateLocation();
        location = await _locationsService.AddAsync(location);

        var payload = new Location("Updated Location");

        location.Patch(payload)
            .Property(x => x.Name);

        // Act
        location = await _locationsService.UpdateAsync(location.ExternalId!.Value, location);

        // Assert
        var filter = new LocationFilterSet { Id = location.Id };
        var updatedLocation = await _locationsRepository.GetAsync(filter);

        Assert.NotNull(updatedLocation);
        Assert.Equal(location.Id, updatedLocation.Id);
        Assert.Equal(location.ExternalId, updatedLocation.ExternalId);
        Assert.Equal(location.Name, updatedLocation.Name);
    }

    [Fact]
    public async Task UpdateLocation_NotExistingLocation_AddsLocation()
    {
        // Arrange
        var location = EntityGenerator.GenerateLocation();

        // Act
        location = await _locationsService.UpdateAsync(location.ExternalId!.Value, location);

        // Assert
        var filter = new LocationFilterSet { Id = location.Id };
        var addedLocation = await _locationsRepository.GetAsync(filter);

        Assert.NotNull(addedLocation);
        Assert.Equal(location.Id, addedLocation.Id);
        Assert.Equal(location.ExternalId, addedLocation.ExternalId);
        Assert.Equal(location.Name, addedLocation.Name);
    }

    [Fact]
    public async Task RemoveLocation_ExistingLocation_RemovesLocation()
    {
        // Arrange
        var location = EntityGenerator.GenerateLocation();
        await _locationsService.AddAsync(location);

        // Act
        await _locationsService.RemoveAsync(location.ExternalId!.Value);

        // Assert
        var filter = new LocationFilterSet { Id = location.Id };
        location = await _locationsRepository.GetAsync(filter);
        Assert.Null(location);
    }

    [Fact]
    public async Task RemoveLocation_NotExistingLocation_DoesNotThrowException()
    {
        // Arrange
        var locationId = Guid.NewGuid();

        // Assert
        Task Action() => _locationsService.RemoveAsync(locationId);
        var exception = await Record.ExceptionAsync(Action);
        Assert.Null(exception);
    }

    [Fact]
    public async Task UpdateStock_NewStock_AddsStock()
    {
        // Arrange
        var location = EntityGenerator.GenerateLocation();
        location = await _locationsService.AddAsync(location);

        var item = EntityGenerator.GenerateItem();
        item = await _itemsService.AddAsync(item);

        var quantity = _random.Next(1, 10);

        // Act
        await _locationsService.UpdateStockAsync(location.ExternalId!.Value, item.ExternalId!.Value, quantity);

        // Assert 
        var itemAvailability = await _itemsService.GetAvailabilityAsync(item.Id!.Value);
        var availabilityInLocations = itemAvailability.AvailableInLocations.ToList();
        var availabilityInLocation = availabilityInLocations.FirstOrDefault();

        Assert.NotNull(itemAvailability);
        Assert.Single(availabilityInLocations);
        Assert.NotNull(availabilityInLocation);
        Assert.Equal(location.Id, availabilityInLocation.LocationId);
        Assert.Equal(quantity, availabilityInLocation.Quantity);
    }

    [Fact]
    public async Task UpdateStock_ExistingStock_UpdatesStock()
    {
        // Arrange
        var location = EntityGenerator.GenerateLocation();
        location = await _locationsService.AddAsync(location);

        var item = EntityGenerator.GenerateItem();
        item = await _itemsService.AddAsync(item);

        var quantity = _random.Next(1, 10);

        await _locationsService.UpdateStockAsync(location.ExternalId!.Value, item.ExternalId!.Value, quantity);

        quantity = _random.Next(1, 10);

        // Act
        await _locationsService.UpdateStockAsync(location.ExternalId!.Value, item.ExternalId!.Value, quantity);

        // Assert
        var itemAvailability = await _itemsService.GetAvailabilityAsync(item.Id!.Value);
        var availabilityInLocations = itemAvailability.AvailableInLocations.ToList();
        var availabilityInLocation = availabilityInLocations.FirstOrDefault();

        Assert.NotNull(itemAvailability);
        Assert.Single(availabilityInLocations);
        Assert.NotNull(availabilityInLocation);
        Assert.Equal(location.Id, availabilityInLocation.LocationId);
        Assert.Equal(quantity, availabilityInLocation.Quantity);
    }

    [Fact]
    public async Task UpdateStock_NotExistingItem_ThrowsNotFoundApiException()
    {
        // Arrange
        var location = EntityGenerator.GenerateLocation();
        await _locationsService.AddAsync(location);

        var itemId = Guid.NewGuid();

        var quantity = _random.Next(1, 10);

        // Assert
        Task Action() => _locationsService.UpdateStockAsync(location.ExternalId!.Value, itemId, quantity);
        await Assert.ThrowsAsync<NotFoundApiException>(Action);
    }

    [Fact]
    public async Task UpdateStock_NotExistingLocation_ThrowsNotFoundApiException()
    {
        // Arrange
        var item = EntityGenerator.GenerateItem();
        await _itemsService.AddAsync(item);

        var locationId = Guid.NewGuid();

        var quantity = _random.Next(1, 10);

        // Assert
        Task Action() => _locationsService.UpdateStockAsync(locationId, item.ExternalId!.Value, quantity);
        await Assert.ThrowsAsync<NotFoundApiException>(Action);
    }

    [Fact]
    public async Task RemoveStock_ExistingItem_RemovesStock()
    {
        // Arrange
        var location = EntityGenerator.GenerateLocation();
        await _locationsService.AddAsync(location);

        var item = EntityGenerator.GenerateItem();
        item = await _itemsService.AddAsync(item);

        var quantity = _random.Next(1, 10);

        await _locationsService.UpdateStockAsync(location.ExternalId!.Value, item.ExternalId!.Value, quantity);

        // Act
        await _locationsService.RemoveStockAsync(location.ExternalId!.Value, item.ExternalId!.Value);

        // Assert
        var itemAvailability = await _itemsService.GetAvailabilityAsync(item.Id!.Value);
        var availabilityInLocations = itemAvailability.AvailableInLocations.ToList();

        Assert.NotNull(itemAvailability);
        Assert.Empty(availabilityInLocations);
    }

    [Fact]
    public async Task RemoveStock_NotExistingItem_ThrowsNotFoundApiException()
    {
        // Arrange
        var location = EntityGenerator.GenerateLocation();
        await _locationsService.AddAsync(location);

        var itemId = Guid.NewGuid();

        // Assert
        Task Action() => _locationsService.RemoveStockAsync(location.ExternalId!.Value, itemId);
        await Assert.ThrowsAsync<NotFoundApiException>(Action);
    }

    [Fact]
    public async Task RemoveStock_NotExistingLocation_ThrowsNotFoundApiException()
    {
        // Arrange
        var locationId = Guid.NewGuid();

        var item = EntityGenerator.GenerateItem();
        await _itemsService.AddAsync(item);

        // Assert
        Task Action() => _locationsService.RemoveStockAsync(locationId, item.ExternalId!.Value);
        await Assert.ThrowsAsync<NotFoundApiException>(Action);
    }
}