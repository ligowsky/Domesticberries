using Dberries.Warehouse.Persistence;
using Microsoft.EntityFrameworkCore;

namespace Dberries.Warehouse.Tests;

internal class TestDbContext : AppDbContext
{
    public TestDbContext(DbContextOptions options) : base(options)
    {
    }
}