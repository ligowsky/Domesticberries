using BitzArt.ApiExceptions;
using Microsoft.Extensions.DependencyInjection;

namespace Dberries.Store.Tests;

[Collection("Service Collection")]
public class UsersServiceTests
{
    private readonly IUsersService _usersService;

    public UsersServiceTests(TestServiceContainer testServiceContainer)
    {
        var serviceProvider = testServiceContainer.ServiceProvider;
        _usersService = serviceProvider.GetRequiredService<IUsersService>();
    }

    [Fact]
    public async Task GetUser_ExistingUser_ReturnsUser()
    {
        // Arrange
        var user = EntityGenerator.GenerateUser();
        var filter = new UserFilterSet { ExternalId = user.ExternalId };
        user = await _usersService.AddAsync(filter, user);
        filter = new UserFilterSet { Id = user.Id };

        // Act
        var returnedUser = await _usersService.GetAsync(filter);

        // Assert
        Assert.NotNull(returnedUser);
        Assert.Equal(user.Id, returnedUser.Id);
        Assert.Equal(user.ExternalId, returnedUser.ExternalId);
        Assert.Equal(user.Email, returnedUser.Email);
    }

    [Fact]
    public async Task GetUser_NotExistingUser_ThrowsNotFoundApiException()
    {
        // Arrange
        var filter = new UserFilterSet { Id = Guid.NewGuid() };
        
        // Assert
        Task Action() => _usersService.GetAsync(filter);
        await Assert.ThrowsAsync<NotFoundApiException>(Action);
    }

    [Fact]
    public async Task AddUser_NewUser_AddsUser()
    {
        // Arrange
        var user = EntityGenerator.GenerateUser();
        var filter = new UserFilterSet { ExternalId = user.ExternalId };

        // Act
        user = await _usersService.AddAsync(filter, user);

        // Assert
        filter = new UserFilterSet { Id = user.Id };
        var returnedUser = await _usersService.GetAsync(filter);

        Assert.NotNull(returnedUser);
        Assert.Equal(user.Id, returnedUser.Id);
        Assert.Equal(user.ExternalId, returnedUser.ExternalId);
        Assert.Equal(user.Email, returnedUser.Email);
    }

    [Fact]
    public async Task AddUser_ExistingUser_ThrowsConflictApiException()
    {
        // Arrange
        var user = EntityGenerator.GenerateUser();
        var filter = new UserFilterSet { ExternalId = user.ExternalId };
        await _usersService.AddAsync(filter, user);
        
        // Assert
        Task Action() => _usersService.AddAsync(filter, user);
        await Assert.ThrowsAsync<ConflictApiException>(Action);
    }
}