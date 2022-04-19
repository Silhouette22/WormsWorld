# WormsWorld

Данный проект был создан в ходе прохождения курса по .NET/C# в НГУ. 

Ниже представлена поставленная задача для текущей ветки. 

## Задача 3. (DI Container)

Симулятор должен работать в .Net Generic Host * как Hosted служба.

### В виде отдельных служб необходимо реализовать:

* Генератор еды.

* Генератор имен червячков.

* Логику действий червячка.

* Службу записи отчета о действиях червячка в файл.

### Точка входа должна выглядеть примерно так:

      class Program
      {
          public static void Main(string[] args)
          {
              CreateHostBuilder(args).Build().Run();
          }
          public static IHostBuilder CreateHostBuilder(string[] args)
          {
              return Host.CreateDefaultBuilder(args)
                    .ConfigureServices((hostContext, services) =>
                    {
                        services.AddHostedService<WorldSimulatorService>();
                        // Служба симулятора мира
                        services.AddScoped<FoodGenerator>();
                        // Генератор еды
                        // ...
                    });
          }
      } 
