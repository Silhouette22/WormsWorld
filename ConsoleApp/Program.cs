﻿using System.IO;
using Microsoft.EntityFrameworkCore;
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
                        .AddSingleton<IBehaviourProvider, BehaviourProvider>()
                        .AddDbContext<MyDbContext>(option => option.UseNpgsql(
                            @"Server=localhost;Database=WormsWorld.Environment;UserId=postgres;Password=password"))
                        .AddSingleton<DbRepository>()
                        .AddSingleton<INameProvider>(new NameProvider(args.Length > 0 ? args[0] : "empty"))
                        .AddScoped<IReportWriter>(_ => new ReportWriter(new StreamWriter("output.txt")))
                        .AddScoped<IFoodGenerator, FoodGenerator>()
                        .AddSingleton<IWormGenerator, WormGenerator>();
                });
        }
    }
}