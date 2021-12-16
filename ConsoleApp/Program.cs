using System;
using System.IO;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;

namespace ConsoleApp
{
    class Program
    {
        public static void Main(string[] args)

        {
            CreateHostBuilder(args).Build().Run();
        }

        private static IHostBuilder CreateHostBuilder(string[] args)
        {
            return Host.CreateDefaultBuilder(args)
                .ConfigureServices((services) =>
                {
                    services.AddHostedService<WorldController>()
                        .AddSingleton<WorldState>()
                        .AddSingleton<IActionProvider, ActionProvider>()
                        .AddScoped<IReportWriter>(_ => new ReportWriter(new StreamWriter("output.txt")))
                        .AddScoped<IFoodGenerator, FoodGenerator>()
                        .AddSingleton<IWormGenerator, WormGenerator>();
                });
        }
    }
}