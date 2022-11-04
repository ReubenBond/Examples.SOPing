using Grains;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using System.Diagnostics;

namespace SOPing.Client;

internal class Program
{
    static async Task Main(string[] args)
    {
        var builder = Host.CreateDefaultBuilder(args)
                .UseOrleansClient(clientBuilder =>
                {
                    clientBuilder.UseLocalhostClustering();
                });
        using var host = await builder.StartAsync();
        var client = host.Services.GetRequiredService<IClusterClient>();
        await DoClientWorkAsync(client);

        await host.StopAsync();
    }

    static async Task DoClientWorkAsync(IClusterClient client)
    {
        const int NumGrains = 1000;
        List<IUserWalletGrain> grains = new(NumGrains);

        foreach (var i in Enumerable.Range(1, NumGrains))
        {
            var walletGrain = client.GetGrain<IUserWalletGrain>(i);
            await walletGrain.Ping(); //make sure grain is loaded
            grains.Add(walletGrain);
        }

        var sw = Stopwatch.StartNew();
        var command = new CreateOrderCommand { Id = Guid.NewGuid(), One = 4, Two = 5, Values = new List<Guid> { Guid.NewGuid(), Guid.NewGuid() } };
        await Parallel.ForEachAsync(Enumerable.Range(1, 100_000), async (o, token) =>
        {
            await grains[o % NumGrains].CreateOrder(command);
        });

        Console.WriteLine($"\nElapsed:{sw.ElapsedMilliseconds}\n\n");
    }
}