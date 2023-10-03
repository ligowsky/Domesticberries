using Dberries.Auth.Persistence;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;

namespace Dberries.Auth.Tests;

[Collection("Service Collection")]
public class DbContextTests
{
    private readonly IServiceProvider _services;

    public DbContextTests(TestServiceContainer testServiceContainer)
    {
        _services = testServiceContainer.ServiceProvider;
    }

    [Fact]
    public async Task UserCreated()
    {
        // Arrange
        var dbContext = _services.GetRequiredService<AppDbContext>();

        //Act
        var item = new User()
        {
            Id = Guid.NewGuid(),
            Email = "user@dberries.com",
            PasswordHash = "passwordHash"
        };

        await dbContext.AddAsync(item);
        await dbContext.SaveChangesAsync();

        var createdItem = await dbContext.Set<User>()
            .Where(x => x.Id == item.Id)
            .FirstOrDefaultAsync();

        //Assert
        Assert.NotNull(createdItem);
        Assert.Equal(item.Id, createdItem.Id);
        Assert.Equal(item.Email, createdItem.Email);
        Assert.Equal(item.PasswordHash, createdItem.PasswordHash);
    }
}