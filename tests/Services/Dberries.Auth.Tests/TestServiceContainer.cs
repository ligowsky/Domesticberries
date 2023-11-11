using Dberries.Auth.Infrastructure;
using Dberries.Auth.Persistence;
using MassTransit;
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
                "MIIEogIBAAKCAQB49iXziVH5ZgB6qelxQ1B6/p8UtDg+ycWXK71zV9TxFeHUKYGclhenbYCNd/bXKqR7hXWU1tYIRp1BmqTkaRkNKQ+kE1tj/LWAlUAru33Um3zimR0eCD7SPcUyqzVXrxY/9Izqao1FO5EyxIzGr9gEYvhs3SjQZUvtLrgK2YhU/ZYFqg+rO27iomc640sZAhgzPe0xyDbPowc/oYTBw76+Rl/pNVKurBjeJIqUM9EgNqs0cFDyuIPggXYs3+lZHtjoMPMaOnCWdENctTOTJA14dzHeROVIK/Q86Wd3DAGTyexAooGYN2VbxcBWOfdcQrGTY7NYAgYhmakrO+G4/mfdAgMBAAECggEAH8FXI0E4oQWzOICkxVRLq45uyuupHGqyEcas3LwBLi11dNJUsXwBx7WKmxkUV1ZdigP2Y1WnKNMI87EaetBQgFD4RzXBt1UffRsxlomJNih/t6lEqJ63h1AqFew2e8d+StnzqJLCdZt2ueI5puuBBV3KJCJHVbZSIXkzXb3P+aefBDUvXqv7pRWfVm0+o+Us8NC1HnKUx52ZOyKS0wwd0ZySM9hMJtnNhIKYlqNg4687ciOc0hUjUco9R00htn1qFJqBGxd8FAPWqEoEQVME4/ygbr75IBhjpusy5Knb8C8pud5MAEuLogsutZ6Nmjs+h3bqg4BcXL6lHvanz92RDQKBgQDVF962nhkLQ0p7Rlu3HudIIgFjilIc7l2dkMFlRfG+2P+ry9Go6N5WS3xDXEKaSAK2yAMhy55cyy73d/uJ7p71bfqe8vJMwVCyLTrbAheXxtjmuEbb/JIufJRC/sH7IZauCmPd4Pdzz/DgwzUkn3tBi5+r504zLXZrdg8RPXRcmwKBgQCRUT7GC4Wo5l+q5tjGHlLs51sws+t7NAKEcLKIenPJBkIMkQ3l/MTXgOePDYG3DzIEvZ9brSqHZhB9Kn4i3M0aqjCHs6Fegmz1CggE++S+Ue+l2QAToq6fTGEyk91HERDjSI+V2rMBySb7RI5ONbiXb3BRYnb4JoyHlvkkPu4I5wKBgQCOIsfZQ4cs7guGQp32fVSD1rrabG7tAUnMSbvWE1t2k0OmkeMdKYb0RJF0VNvQEOxOgwcjya+t8ZE4W/2aOWIqDtu2nhJkhCWRU34Ii1K1WFthOdV58vSGsW4racZ1/ieFm0SVwPMNeswO/+Y4sXADfmBYx4ZvuIlekHboZvdwtQKBgDgEOI2BPYnJHFG14RQl9lNqL6XLhVedyeahxsCDa4SvS+CqPdBNKtfj0d88FqepTN+09OwTCZTeDDcjnTH1KX9A4ao93VOgNT4UGtlI/Hk8/oNQ7g2jpWq0t7cWdrMBaiAjBW5/uodSshsj5zNQ1BJUG9jTewbXXU6bLPkC5SE7AoGAMV5hmnJgbj/EFTlhFTwflGCDci7KVzOgjOKjTGrWrzmZFoSqofKk49a50YVJH/griJH/mgnHrC0LtNqQZZrNOd1qOV4qckydyaH19SGXDM41j3X68xJMK10+3JZTEsP4H/FekrMODXB51OxXUtB3jgYxtPUFSrlcrWH9jwk9Bhs=",
            PublicKey = "MIIBITANBgkqhkiG9w0BAQEFAAOCAQ4AMIIBCQKCAQB49iXziVH5ZgB6qelxQ1B6/p8UtDg+ycWXK71zV9TxFeHUKYGclhenbYCNd/bXKqR7hXWU1tYIRp1BmqTkaRkNKQ+kE1tj/LWAlUAru33Um3zimR0eCD7SPcUyqzVXrxY/9Izqao1FO5EyxIzGr9gEYvhs3SjQZUvtLrgK2YhU/ZYFqg+rO27iomc640sZAhgzPe0xyDbPowc/oYTBw76+Rl/pNVKurBjeJIqUM9EgNqs0cFDyuIPggXYs3+lZHtjoMPMaOnCWdENctTOTJA14dzHeROVIK/Q86Wd3DAGTyexAooGYN2VbxcBWOfdcQrGTY7NYAgYhmakrO+G4/mfdAgMBAAE=",
            Salt = "93c5ee728770429d8a34b5874c358655",
            AccessTokenDurationInMinutes = 60,
            RefreshTokenDurationInMinutes = 10080
        };

        services.AddSingleton(authOptions);

        var sqliteConnection = new SqliteConnection("Filename=:memory:");
        sqliteConnection.Open();

        services.AddDbContext<TestDbContext>(x => x.UseSqlite(sqliteConnection));
        services.AddScoped<AppDbContext>(x => x.GetRequiredService<TestDbContext>());
        services.AddRepositories();
        services.AddServices();

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