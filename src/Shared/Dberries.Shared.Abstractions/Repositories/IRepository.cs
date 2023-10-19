namespace Dberries;

public interface IRepository
{
    Task<int> SaveChangesAsync();
}