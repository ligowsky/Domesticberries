using Microsoft.EntityFrameworkCore;

namespace Dberries.Auth.Persistence;

public abstract class AppDbContext : DbContext
{
    protected AppDbContext(DbContextOptions options) : base(options)
    {
    }
}