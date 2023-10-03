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
        var user = new User()
        {
            Id = Guid.NewGuid(),
            Email = "user@dberries.com",
            PasswordHash = "passwordHash"
        };

        await dbContext.AddAsync(user);
        await dbContext.SaveChangesAsync();

        var createdUser = await dbContext.Set<User>()
            .Where(x => x.Id == user.Id)
            .FirstOrDefaultAsync();

        //Assert
        Assert.NotNull(createdUser);
        Assert.Equal(user.Id, createdUser.Id);
        Assert.Equal(user.Email, createdUser.Email);
        Assert.Equal(user.PasswordHash, createdUser.PasswordHash);
    }
}