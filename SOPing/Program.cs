using Microsoft.Extensions.Hosting;
using System.Net;

ServicePointManager.UseNagleAlgorithm = false;

var builder = Host.CreateDefaultBuilder(args)
    .UseOrleans(siloBuilder =>
    {
        siloBuilder.UseLocalhostClustering()
            .AddMemoryGrainStorage("OrleansMemoryProvider");
    });

await builder.RunConsoleAsync();