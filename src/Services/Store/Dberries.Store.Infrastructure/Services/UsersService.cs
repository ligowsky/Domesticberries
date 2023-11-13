using BitzArt;

namespace Dberries.Store.Infrastructure;

public class UsersService : IUsersService
{
    private readonly IUsersRepository _usersRepository;

    public UsersService(IUsersRepository usersRepository)
    {
        _usersRepository = usersRepository;
    }

    public async Task<User> GetAsync(IFilterSet<User> filterSet)
    {
        var user = await _usersRepository.GetAsync(filterSet);

        if (user is null)
            throw ApiException.NotFound($"{nameof(User)} is not found");

        return user;
    }

    public async Task<User> AddAsync(IFilterSet<User> filterSet, User user)
    {
        await _usersRepository.AddAsync(user);
        await _usersRepository.SaveChangesAsync();

        return await GetAsync(filterSet);
    }
}