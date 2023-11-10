using Dberries.Auth.Infrastructure;
using Dberries.Auth.Persistence;
using Microsoft.Data.Sqlite;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;

namespace Dberries.Auth.Tests;

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

        var authOptions = new TokenAuthProviderOptions
        {
            PrivateKey =
                "MIIEpAIBAAKCAQEAjk2s/p+2kV22ALoaW3PQiirybEvu+ALzPMO2hITyckPOrjf6p7J77Vc00+Pna4E4e5ZIxsy7h5+VWSL9Y3S8ZliIT/1G6chmQmr6zzrRwLB7QkQWSpKT7MXp+YZbag+0MKu1KJfdAngnKFFq7Ztc9VxEgYDpYhRZ8dXVbcppTpy3rgYsqE/EtpuCmUNBPnpAzgKzvNZ2CeigVl1f4/n/9Vl6YPUPAj9etnQD3vq7xHdmxUs5Sf5RpXCEG2QWSG5/e6NYx/V6dPoiCmZrTm10mqagoVJAGIRlpGApF6CDqgKtdA2+c2JM9eaQV8P+uDgeYcO/PvCqrY2dxheEBm/D2QIDAQABAoIBABOyPqKTmqJuNRXOrH1B/3hQT3Ob4vyi8+XSNP4MpL4aEo5coy3471w/eMlnPw3LOfUpLPOPPNmdxf8rb6UHcFg+Isfnh5wuJ7FDu2lZe9TO2JCoeelidmUMU8E8zoRUnh0qdw2iT23bEZhsBH/UxD4VwkmmfScdKAHoFf13SbIem5hP75iqPvwAvhISRIxI+2Wmx47Sc/qfXADzpQ3IKMG1RJjk9lfoUICeYcyI5uCHtw49UzFPmilqV4+LnnTJcK6Lg95m0P3d4PjouhSpHbzJ3WrqfSjr3FkS+3ibuYCjxvX2ZQs0hS81Ma/65TWwHSlPVQx9hO3uDZuQq6nu/IECgYEA22O3ETxzbtRx7mc3j1SxiZ33J8GvWWAH9OGJp+HE65+6xur2alop7Bkeuk7fB7UcGU1m7FaWmKfYD3kC82E4cXbOymC3qEzdJ0laLcSIsOl65bvO1Ehx0s52KXp9EwRkpk2MwLSkYI9RxtHmBasfanRIA5ZRymxpLPhB3TrVYKkCgYEApgza66cv93I+ENtBC4B5OUW251imvJDyvgR8Zn30NeTYSwkgnZOaukzDXmvSy3jOCTN4rbvQYkrv8aHA5JP1siEL6m/bp4XBvjIykN3gS0zwLR3KT+sGF4lPjaps5fRnmW5uZJGDv09YDoxtIwAiMIsPWedByePeqUIoMaEq17ECgYEArinz++fsj5BxvdwM/Gk049YWjmYxSRd2WY3ulOsjRBay1RVUd8uKOlca0LaqUdkSLHgI/BglR5z/ZhEgKYwFnfW+ZmTvh9k1O+n8gBbkmOVRXi2BHX9BdSPYJEeMIVu8d1VbJuSBFfLQ7bBWFivqLb2QKeDcn4D+4edXikQZfrkCgYEAi/VT5MKzfGbPh5e2eIJeKvbm1pJzX4bxA1Z2oFDTsUVlRcG4uvi5twOSvJg3QYdeaoT218DxQJgXLFyBYGiR9AVErOKBiu8DwVNAKbjKH84GtpBggQIAzX/QDQEz7pMVa06YP64jFPJEr2aPaqvnJXFgjA+O9SNa/uEjTcmhedECgYAxiJpI+e0q3K9Ys8R+yrWra8YyQ4wGLHggOoJTt925vvSpBRvsheY3ft9yvDhl/64UUyKcqOUeH3FB1I9eTWvYAfdfy1aVSvEJG8NdylbamcqcO/bdKO/MJSByCoP8yS375T//NSO0O5eUmtUbo8TyR99PLEmGxegRSlbaOSmAwg==",
            PublicKey = "MIIBIjANBgkqhkiG9w0BAQEFAAOCAQ8AMIIBCgKCAQEAjk2s/p+2kV22ALoaW3PQiirybEvu+ALzPMO2hITyckPOrjf6p7J77Vc00+Pna4E4e5ZIxsy7h5+VWSL9Y3S8ZliIT/1G6chmQmr6zzrRwLB7QkQWSpKT7MXp+YZbag+0MKu1KJfdAngnKFFq7Ztc9VxEgYDpYhRZ8dXVbcppTpy3rgYsqE/EtpuCmUNBPnpAzgKzvNZ2CeigVl1f4/n/9Vl6YPUPAj9etnQD3vq7xHdmxUs5Sf5RpXCEG2QWSG5/e6NYx/V6dPoiCmZrTm10mqagoVJAGIRlpGApF6CDqgKtdA2+c2JM9eaQV8P+uDgeYcO/PvCqrY2dxheEBm/D2QIDAQAB",
            Salt = "93c5ee728770429d8a34b5874c358655",
            AccessTokenDurationInMinutes = 60
        };

        services.AddSingleton(authOptions);

        var sqliteConnection = new SqliteConnection("Filename=:memory:");
        sqliteConnection.Open();

        services.AddDbContext<TestDbContext>(x => x.UseSqlite(sqliteConnection));
        services.AddScoped<AppDbContext>(x => x.GetRequiredService<TestDbContext>());
        services.AddRepositories();
        services.AddServices();

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