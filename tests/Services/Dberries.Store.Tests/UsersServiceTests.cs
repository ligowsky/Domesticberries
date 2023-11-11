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
        await _usersService.AddAsync(user);

        // Assert
        var returnedUser = await _usersService.GetAsync(user.Id!.Value);

        Assert.NotNull(returnedUser);
        Assert.Equal(user.Id, returnedUser.Id);
        Assert.Equal(user.ExternalId, returnedUser.ExternalId);
        Assert.Equal(user.Email, returnedUser.Email);
    }

    [Fact]
    public async Task GetUser_NotExistingUser_ThrowsNotFoundApiException()
    {
        // Arrange
        var userId = Guid.NewGuid();

        // Assert
        Task Action() => _usersService.GetAsync(userId);
        await Assert.ThrowsAsync<NotFoundApiException>(Action);
    }

    [Fact]
    public async Task AddUser_NewUser_AddsUser()
    {
        // Arrange
        var user = EntityGenerator.GenerateUser();

        // Act
        await _usersService.AddAsync(user);

        // Assert
        var addedUser = await _usersService.GetAsync(user.Id!.Value);

        Assert.NotNull(addedUser);
        Assert.Equal(user.ExternalId, addedUser.ExternalId);
        Assert.Equal(user.Email, addedUser.Email);
    }

    [Fact]
    public async Task AddUser_ExistingUser_ThrowsBadRequestApiException()
    {
        // Arrange
        var user = EntityGenerator.GenerateUser();
        await _usersService.AddAsync(user);

        // Assert
        Task Action() => _usersService.AddAsync(user);
        await Assert.ThrowsAsync<BadRequestApiException>(Action);
    }
}