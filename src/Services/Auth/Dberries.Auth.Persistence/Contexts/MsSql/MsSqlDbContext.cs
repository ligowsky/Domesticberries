using Microsoft.EntityFrameworkCore;

namespace Dberries.Auth.Persistence;

public class MsSqlDbContext : AppDbContext
{
    public MsSqlDbContext(DbContextOptions options) : base(options)
    {
    }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);
        modelBuilder.ApplyConfigurationsFromAssembly(typeof(MsSqlDbContext).Assembly);
    }
}