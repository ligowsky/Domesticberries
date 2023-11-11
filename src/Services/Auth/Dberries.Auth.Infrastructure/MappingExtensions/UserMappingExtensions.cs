namespace Dberries.Auth.Infrastructure;

public static class UserMappingExtensions
{
    public static UserDto ToDto(this User model)
    {
        return new UserDto(model.Id, model.Email);
    }
}