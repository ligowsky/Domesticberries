namespace Dberries.Auth;

public record UserAddedMessage
{
    public UserDto User { get; set; }

    public UserAddedMessage(UserDto user)
    {
        User = user;
    }

    public UserAddedMessage()
    {
    }
}