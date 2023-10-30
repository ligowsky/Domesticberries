namespace Dberries;

public interface IRepository
{
    public Task<int> SaveChangesAsync();
}