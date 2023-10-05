using Microsoft.EntityFrameworkCore;

namespace Dberries.Store.Persistence;

public abstract class AppDbContext : DbContext
{
    protected AppDbContext(DbContextOptions options) : base(options)
    {
    }
}