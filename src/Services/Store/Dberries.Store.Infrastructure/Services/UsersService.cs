namespace Dberries.Store.Infrastructure;

public class UsersService : IUsersService
{
    private readonly IUsersRepository _usersRepository;

    public UsersService(IUsersRepository usersRepository)
    {
        _usersRepository = usersRepository;
    }

    public async Task<User> GetAsync(Guid id)
    {
        return await _usersRepository.GetAsync(id);
    }

    public async Task<User> AddAsync(User user)
    {
        await _usersRepository.AddAsync(user);
        await _usersRepository.SaveChangesAsync();

        return user;
    }
}