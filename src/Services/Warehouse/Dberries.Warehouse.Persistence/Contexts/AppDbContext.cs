using Microsoft.EntityFrameworkCore;

namespace Dberries.Warehouse.Persistence;

public abstract class AppDbContext : DbContext
{
    protected AppDbContext(DbContextOptions options) : base(options)
    {
    }
}