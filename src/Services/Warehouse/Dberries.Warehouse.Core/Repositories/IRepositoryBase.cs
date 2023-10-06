namespace Dberries.Warehouse;

public interface IRepositoryBase
{
    Task<int> SaveChangesAsync();
}