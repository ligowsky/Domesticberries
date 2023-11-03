namespace Dberries;

public abstract class RepositoryBase : IRepository
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