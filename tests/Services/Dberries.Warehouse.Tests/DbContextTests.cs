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
    public async Task AddItem_NewItem_AddsToDatabase()
    {
        // Arrange
        var dbContext = _services.GetRequiredService<AppDbContext>();
        var item = new Item()
        {
            Id = Guid.NewGuid(),
            Name = "Item 1",
            Description = "Description 1"
        };

        //Act
        await dbContext.AddAsync(item);
        await dbContext.SaveChangesAsync();

        //Assert
        var createdItem = await dbContext.Set<Item>()
            .Where(x => x.Id == item.Id)
            .FirstOrDefaultAsync();

        Assert.NotNull(createdItem);
        Assert.Equal(item.Id, createdItem.Id);
        Assert.Equal(item.Name, createdItem.Name);
        Assert.Equal(item.Description, createdItem.Description);
    }
}