using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;

namespace Dberries.Warehouse.Persistence;

public class MsSqlDbContextDesignTimeFactory : IDesignTimeDbContextFactory<MsSqlDbContext>
{
    public MsSqlDbContext CreateDbContext(string[] args)
    {
        var builder = new DbContextOptionsBuilder<MsSqlDbContext>();

        builder.UseSqlServer("Server=localhost;Database=warehouse;User=sa;Password=Str0ngP@ssw0rd;Encrypt=False");
        return new MsSqlDbContext(builder.Options);
    }
}