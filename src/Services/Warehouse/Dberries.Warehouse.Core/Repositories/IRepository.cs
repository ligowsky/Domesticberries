namespace Dberries.Warehouse;

public interface IRepository
{
    Task<int> SaveChangesAsync();
}