using BitzArt.ApiExceptions;
using Microsoft.Extensions.DependencyInjection;

namespace Dberries.Auth.Tests;

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
    public async Task Signup_NewUser_ReturnsAuthResponse()
    {
        // Arrange
        var request = EntityGenerator.GenerateAuthRequest();
        
        // Assert
        var response = await _usersService.SignUpAsync(request);
        
        Assert.NotNull(response);
        Assert.NotNull(response.AccessToken);
    }
    
    [Fact]
    public async Task Signup_ExistingUser_ThrowsConflictApiException()
    {
        // Arrange
        var request = EntityGenerator.GenerateAuthRequest();
        await _usersService.SignUpAsync(request);

        // Assert
        Task Action() => _usersService.SignUpAsync(request);
        await Assert.ThrowsAsync<ConflictApiException>(Action);
    }
    
    [Fact]
    public async Task Signin_ExistingUser_ReturnsAuthResponse()
    {
        // Arrange
        var request = EntityGenerator.GenerateAuthRequest();
        await _usersService.SignUpAsync(request);
        
        // Assert
        var response = await _usersService.SignInAsync(request);
        
        Assert.NotNull(response);
        Assert.NotNull(response.AccessToken);
    }
    
    [Fact]
    public async Task Signin_NotExistingUser_ThrowsUnauthorizedApiException()
    {
        // Arrange
        var request = EntityGenerator.GenerateAuthRequest();

        // Assert
        Task Action() => _usersService.SignInAsync(request);
        await Assert.ThrowsAsync<UnauthorizedApiException>(Action);
    }
    
    [Fact]
    public async Task Signin_InvalidPassword_ThrowsUnauthorizedApiException()
    {
        // Arrange
        var request = EntityGenerator.GenerateAuthRequest();
        await _usersService.SignUpAsync(request);
        request.Password = "Inv@l1dP@ssw0rd";

        // Assert
        Task Action() => _usersService.SignInAsync(request);
        await Assert.ThrowsAsync<UnauthorizedApiException>(Action);
    }
}