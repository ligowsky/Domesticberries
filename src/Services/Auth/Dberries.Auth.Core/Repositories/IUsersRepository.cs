namespace Dberries.Auth;

public interface IUsersRepository : IRepository
{
    public Task<User> GetAsync(Guid id);
    public Task<User> GetByEmailAsync(string email);
    public Task<bool> ThrowIfExistsByEmailAsync(string email);
    public User Add(User user);
}