using ConsoleApp;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;

namespace BehaviourGeneratorApp
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
                    services.AddHostedService<BehaviourGeneratorService>()
                        .AddDbContext<MyDbContext>(option => option.UseNpgsql(
                            @"Server=localhost;Database=WormsWorld.Environment;UserId=postgres;Password=password"))
                        .AddSingleton<DbRepository>()
                        .AddSingleton<IBehaviourGenerator, BehaviourGenerator>()
                        .AddSingleton<INameProvider>(new NameProvider(args.Length > 0 ? args[0] : "empty"));
                });
        }
    }
}