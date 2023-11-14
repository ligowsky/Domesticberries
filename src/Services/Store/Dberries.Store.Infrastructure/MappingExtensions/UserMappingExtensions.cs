namespace Dberries.Store;

public static class UserMappingExtensions
{
    public static UserDto ToDto(this User model)
    {
        return new UserDto(model.Id, model.Email);
    }

    public static User ToModel(this UserDto dto)
    {
        return new User
        {
            ExternalId = dto.Id,
            Email = dto.Email
        };
    }
}