using Dberries.Warehouse.Infrastructure;
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
        
        services.AddDbContext<TestDbContext>(x => x.UseSqlite(sqliteConnection));
        services.AddScoped<AppDbContext>(x => x.GetRequiredService<TestDbContext>());
        services.AddInfrastructure();
        services.AddRepositories();
        
        _services = services.BuildServiceProvider();
        
        using (var scope = _services.CreateScope())
        {
            var dbContext = scope.ServiceProvider.GetRequiredService<AppDbContext>();
            dbContext.Database.EnsureCreated();
        }
    }
    
    public void Dispose()
    {
        GC.SuppressFinalize(this);
    }
}