using BitzArt.ApiExceptions;
using MassTransit.Testing;
using Microsoft.Extensions.DependencyInjection;

namespace Dberries.Auth.Tests;

[Collection("Service Collection")]
public class UserMessagesTests
{
    private readonly IUsersService _usersService;
    private readonly ITestHarness _harness;

    public UserMessagesTests(TestServiceContainer testServiceContainer)
    {
        var serviceProvider = testServiceContainer.ServiceProvider;
        _usersService = serviceProvider.GetRequiredService<IUsersService>();
        _harness = serviceProvider.GetRequiredService<ITestHarness>();
    }

    [Fact]
    public async Task Signup_NewUser_PublishesMessage()
    {
        // Arrange
        var request = EntityGenerator.GenerateAuthRequest();

        // Act
        await _usersService.SignUpAsync(request);

        // Assert
        var message = _harness.Published
            .Select<UserAddedMessage>()
            .FirstOrDefault(x => x.Context.Message.User.Email == request.Email)?
            .Context.Message;

        Assert.NotNull(message);
        Assert.NotNull(message.User);
        Assert.Equal(request.Email, message.User.Email);
    }
}