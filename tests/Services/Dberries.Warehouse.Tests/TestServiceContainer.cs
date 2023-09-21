using Dberries.Warehouse.Persistence;
using Microsoft.Data.Sqlite;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;

namespace Dberries.Warehouse.Tests;

[CollectionDefinition("Service Collection")]
public class ContainerCollection : ICollectionFixture<TestServiceContainer> { }

public class TestServiceContainer : IDisposable
{
    private readonly IServiceProvider _services;
    public IServiceProvider ServiceProvider => _services.CreateScope().ServiceProvider;
    
    public TestServiceContainer()
    {
        var services = new ServiceCollection();  
        
        var sqliteConnection = new SqliteConnection("Filename=:memory:");
        sqliteConnection.Open();
        
        var options = new DbContextOptionsBuilder<TestDbContext>()
            .UseSqlite(sqliteConnection)
            .Options;
        
        using (var context = new TestDbContext(options))
        {
            context.Database.EnsureCreated();
        }
        
        services.AddDbContext<TestDbContext>(x => x.UseSqlite(sqliteConnection));
        services.AddScoped<AppDbContext>(x => x.GetRequiredService<TestDbContext>());
        
        _services = services.BuildServiceProvider();
    }
    
    public void Dispose()
    {
        GC.SuppressFinalize(this);
    }
}