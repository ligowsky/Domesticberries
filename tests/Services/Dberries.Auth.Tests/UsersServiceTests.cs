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
        Assert.NotNull(response.RefreshToken);
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
        Assert.NotNull(response.RefreshToken);
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
    
    [Fact]
    public async Task RefreshToken_ValidToken_ReturnsAuthResponse()
    {
        // Arrange
        var authRequest = EntityGenerator.GenerateAuthRequest();
        var authResponse = await _usersService.SignUpAsync(authRequest);
        var refreshToken = authResponse.RefreshToken;
        var refreshTokenRequest = new RefreshTokenRequestDto(refreshToken!);
        
        // Assert
        var refreshTokenResponse = _usersService.RefreshTokenAsync(refreshTokenRequest);
        
        Assert.NotNull(refreshTokenResponse);
        Assert.NotNull(refreshTokenResponse.AccessToken);
        Assert.NotNull(refreshTokenResponse.RefreshToken);
    }
    
    [Fact]
    public async Task RefreshToken_InvalidToken_ReturnsAuthResponse()
    {
        // Arrange
        var authRequest = EntityGenerator.GenerateAuthRequest();
        await _usersService.SignUpAsync(authRequest);
        const string refreshToken = "InvalidRefreshToken";
        var refreshTokenRequest = new RefreshTokenRequestDto(refreshToken);
        
        // Assert
        void Action() => _usersService.RefreshTokenAsync(refreshTokenRequest); 
        Assert.Throws<UnauthorizedApiException>(Action);
    }
}