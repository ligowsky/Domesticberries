using Dberries.Warehouse.Core;
using Dberries.Warehouse.Persistence;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;

namespace Dberries.Warehouse.Tests;

[Collection("Service Collection")]
public class DbContextTests
{
    private readonly IServiceProvider _services;

    public DbContextTests(TestServiceContainer testServiceContainer)
    {
        _services = testServiceContainer.ServiceProvider;
    }

    [Fact]
    public async Task ItemCreated()
    {
        // Arrange
        var dbContext = _services.GetRequiredService<AppDbContext>();

        //Act
        var item = new Item()
        {
            Id = Guid.NewGuid(),
            Name = "Item 1",
            Description = "Description 1"
        };

        await dbContext.AddAsync(item);
        await dbContext.SaveChangesAsync();

        var createdItem = await dbContext.Set<Item>()
            .Where(x => x.Id == item.Id)
            .FirstOrDefaultAsync();

        //Assert
        Assert.NotNull(createdItem);
        Assert.Equal(item.Id, createdItem.Id);
        Assert.Equal(item.Name, createdItem.Name);
        Assert.Equal(item.Description, createdItem.Description);
    }
}