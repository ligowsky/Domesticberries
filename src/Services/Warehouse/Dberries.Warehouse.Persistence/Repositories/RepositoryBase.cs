namespace Dberries.Warehouse.Persistence;

public class RepositoryBase : IRepositoryBase
{
    protected readonly AppDbContext Db;

    protected RepositoryBase(AppDbContext db)
    {
        Db = db;
    }

    public async Task<int> SaveChangesAsync()
    {
        return await Db.SaveChangesAsync();
    }
}