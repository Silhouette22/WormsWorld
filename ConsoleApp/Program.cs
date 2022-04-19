using System.IO;
using ActionProviderLib;
using BehaviourProviderLib;
using DatabaseLib;
using FoodGeneratorLib;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using NameProviderLib;
using WorldStateLib;
using WormGeneratorLib;

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
                        // ***** Action Source *****
                        .AddSingleton<IActionProvider>(new HttpActionProvider(
                            "http://localhost:5000/WormsWorld/ActionProvider"))
                        // .AddSingleton<IActionProvider, ActionProvider>()
                        // ***** Using Database *****
                        .AddSingleton<IBehaviourProvider, BehaviourProvider>()
                        .AddDbContext<MyDbContext>(option => option.UseNpgsql(
                            @"Server=localhost;Database=WormsWorld.Environment;UserId=postgres;Password=password"))
                        .AddSingleton<DbRepository>()
                        .AddSingleton<INameProvider>(new NameProvider(args.Length > 0 ? args[0] : "empty"))
                        // ***** Other *****
                        .AddScoped<IReportWriter>(_ => new ReportWriter(new StreamWriter("output.txt")))
                        .AddScoped<IFoodGenerator, FoodGenerator>()
                        .AddSingleton<IWormGenerator, WormGenerator>();
                });
        }
    }
}