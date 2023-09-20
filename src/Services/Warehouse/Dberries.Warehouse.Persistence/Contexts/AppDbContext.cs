using Microsoft.EntityFrameworkCore;

namespace Warehouse.Persistence;

public abstract class AppDbContext : DbContext
{
    protected AppDbContext(DbContextOptions options) : base(options)
    {
    }
}