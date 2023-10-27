using Microsoft.EntityFrameworkCore;

namespace Dberries;

public abstract class AppDbContext : DbContext
{
    protected AppDbContext(DbContextOptions options) : base(options)
    {
    }
}