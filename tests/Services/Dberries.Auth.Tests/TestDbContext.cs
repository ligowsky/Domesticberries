using Dberries.Auth.Persistence;
using Microsoft.EntityFrameworkCore;

namespace Dberries.Auth.Tests;

internal class TestDbContext : AppDbContext
{
    public TestDbContext(DbContextOptions options) : base(options)
    {
    }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);
        modelBuilder.ApplyConfigurationsFromAssembly(typeof(MsSqlDbContext).Assembly);
    }
}