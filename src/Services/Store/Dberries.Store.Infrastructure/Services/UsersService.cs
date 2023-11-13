using BitzArt;

namespace Dberries.Store.Infrastructure;

public class UsersService : IUsersService
{
    private readonly IUsersRepository _usersRepository;

    public UsersService(IUsersRepository usersRepository)
    {
        _usersRepository = usersRepository;
    }

    public async Task<User> GetAsync(IFilterSet<User> filter)
    {
        var user = await _usersRepository.GetAsync(filter);

        if (user is null)
            throw ApiException.NotFound($"{nameof(User)} is not found");

        return user;
    }

    public async Task<User> AddAsync(IFilterSet<User> filter, User user)
    {
        var existingUser = await _usersRepository.GetAsync(filter);

        if (existingUser is not null)
            throw ApiException.Conflict($"{nameof(User)} already exists");

        _usersRepository.Add(user);
        await _usersRepository.SaveChangesAsync();

        return user;
    }
}