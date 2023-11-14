using Dberries.Auth;
using MassTransit;

namespace Dberries.Store.WebAPI;

public class UserAddedMessageConsumer : IConsumer<UserAddedMessage>
{
    private readonly IUsersService _usersService;

    public UserAddedMessageConsumer(IUsersService usersService)
    {
        _usersService = usersService;
    }

    public async Task Consume(ConsumeContext<UserAddedMessage> context)
    {
        var user = context.Message.User.ToModel();
        await _usersService.AddAsync(user);
    }
}