using BitzArt.ApiExceptions;
using MassTransit.Testing;
using Microsoft.Extensions.DependencyInjection;

namespace Dberries.Warehouse.Tests;

[Collection("Service Collection")]
public class LocationsMessagesTests
{
    private readonly ILocationsService _locationsService;
    private readonly IItemsService _itemsService;
    private readonly ITestHarness _harness;

    public LocationsMessagesTests(TestServiceContainer testServiceContainer)
    {
        var serviceProvider = testServiceContainer.ServiceProvider;
        _locationsService = serviceProvider.GetRequiredService<ILocationsService>();
        _itemsService = serviceProvider.GetRequiredService<IItemsService>();
        _harness = serviceProvider.GetRequiredService<ITestHarness>();
    }

    [Fact]
    public async Task AddLocation_NewLocation_MessagePublished()
    {
        // Arrange
        var location = EntityGenerator.GenerateLocation();

        // Act
        location = await _locationsService.AddAsync(location);

        // Assert
        var message = _harness.Published
            .Select<LocationAddedMessage>()
            .FirstOrDefault(x => x.Context.Message.Location.Id == location.Id)?.Context.Message;

        Assert.NotNull(message);
        Assert.Equal(location.Id, message.Location.Id);
        Assert.Equal(location.Name, message.Location.Name);
        Assert.Equal(location.Coordinates?.Latitude, message.Location.Coordinates?.Latitude);
        Assert.Equal(location.Coordinates?.Longitude, message.Location.Coordinates?.Longitude);
    }

    [Fact]
    public async Task UpdateLocation_ExistingLocation_MessagePublished()
    {
        // Arrange
        var location = EntityGenerator.GenerateLocation();
        location = await _locationsService.AddAsync(location);

        // Act
        location = await _locationsService.UpdateAsync(location.Id!.Value, location);

        // Assert
        var message = _harness.Published
            .Select<LocationUpdatedMessage>()
            .FirstOrDefault(x => x.Context.Message.Location.Id == location.Id)?.Context.Message;

        Assert.NotNull(message);
        Assert.Equal(location.Id, message.Location.Id);
        Assert.Equal(location.Name, message.Location.Name);
        Assert.NotNull(location.Coordinates);
        Assert.NotNull(message.Location.Coordinates);
        Assert.Equal(location.Coordinates.Latitude, message.Location.Coordinates.Latitude);
        Assert.Equal(location.Coordinates.Longitude, message.Location.Coordinates.Longitude);
    }

    [Fact]
    public async Task UpdateLocation_NotExistingLocation_MessageNotPublished()
    {
        // Arrange
        var locationId = Guid.NewGuid();
        var location = EntityGenerator.GenerateLocation();
        Task Action() => _locationsService.UpdateAsync(locationId, location);

        // Assert
        await Assert.ThrowsAsync<NotFoundApiException>(Action);

        var isMessagePublished = _harness.Published
            .Select<LocationUpdatedMessage>()
            .Any(x => x.Context.Message.Location.Id == location.Id);

        Assert.False(isMessagePublished);
    }

    [Fact]
    public async Task RemoveLocation_ExistingLocation_MessagePublished()
    {
        // Arrange
        var location = EntityGenerator.GenerateLocation();
        location = await _locationsService.AddAsync(location);

        // Act
        await _locationsService.RemoveAsync(location.Id!.Value);

        // Assert
        var message = _harness.Published
            .Select<LocationRemovedMessage>()
            .FirstOrDefault(x => x.Context.Message.Id == location.Id)?
            .Context.Message;

        Assert.NotNull(message);
        Assert.Equal(location.Id, message.Id);
    }

    [Fact]
    public async Task RemoveLocation_NotExistingLocation_MessageNotPublished()
    {
        // Arrange
        var locationId = Guid.NewGuid();
        Task Action() => _locationsService.RemoveAsync(locationId);

        // Assert
        await Assert.ThrowsAsync<NotFoundApiException>(Action);

        var isMessagePublished = _harness.Published
            .Select<LocationRemovedMessage>()
            .Any(x => x.Context.Message.Id == locationId);

        Assert.False(isMessagePublished);
    }

    [Fact]
    public async Task UpdateStock_NewStock_MessagePublished()
    {
        // Arrange
        var location = EntityGenerator.GenerateLocation();
        location = await _locationsService.AddAsync(location);

        var item = EntityGenerator.GenerateItem();
        item = await _itemsService.AddAsync(item);

        const int quantity = 100;
        var stock = EntityGenerator.GenerateStock(quantity);

        // Act
        await _locationsService.UpdateStockAsync(location.Id!.Value, item.Id!.Value, stock);

        // Assert
        var message = _harness.Published
            .Select<StockUpdatedMessage>()
            .FirstOrDefault(x =>
                x.Context.Message.LocationId == location.Id &&
                x.Context.Message.Stock?.Item?.Id == item.Id
            )?.Context.Message;

        Assert.NotNull(message);
        Assert.Equal(location.Id, message.LocationId);
        Assert.NotNull(message.Stock);
        Assert.Equal(item.Id, message.Stock!.Item?.Id);
        Assert.Equal(item.Name, message.Stock!.Item?.Name);
        Assert.Equal(quantity, message.Stock!.Quantity);
    }

    [Fact]
    public async Task UpdateStock_NotExistingLocation_MessageNotPublished()
    {
        // Arrange
        var item = EntityGenerator.GenerateItem();
        item = await _itemsService.AddAsync(item);

        var locationId = Guid.NewGuid();

        var stock = EntityGenerator.GenerateStock();

        Task Action() => _locationsService.UpdateStockAsync(locationId, item.Id!.Value, stock);

        // Assert
        await Assert.ThrowsAsync<NotFoundApiException>(Action);

        var isMessagePublished = _harness.Published
            .Select<StockUpdatedMessage>()
            .Any(x =>
                x.Context.Message.LocationId == locationId &&
                x.Context.Message.Stock?.Item?.Id == item.Id
            );

        Assert.False(isMessagePublished);
    }

    [Fact]
    public async Task UpdateStock_NotExistingItem_MessageNotPublished()
    {
        // Arrange
        var location = EntityGenerator.GenerateLocation();
        location = await _locationsService.AddAsync(location);

        var itemId = Guid.NewGuid();

        var stock = EntityGenerator.GenerateStock();

        Task Action() => _locationsService.UpdateStockAsync(location.Id!.Value, itemId, stock);

        // Assert
        await Assert.ThrowsAsync<NotFoundApiException>(Action);

        var isMessagePublished = _harness.Published
            .Select<StockUpdatedMessage>()
            .Any(x =>
                x.Context.Message.LocationId == location.Id &&
                x.Context.Message.Stock?.Item?.Id == itemId
            );

        Assert.False(isMessagePublished);
    }
}