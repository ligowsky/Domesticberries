using Dberries.Store.Infrastructure;
using Dberries.Store.Persistence;
using MassTransit;
using Microsoft.Data.Sqlite;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;

namespace Dberries.Store.Tests;

[CollectionDefinition("Service Collection")]
public class ContainerCollection : ICollectionFixture<TestServiceContainer>
{
}

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
        services.AddServices();
        services.AddRepositories();

        services.AddMassTransitTestHarness();

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