using System;
using Microsoft.Extensions.DependencyInjection;
using SEM_5.Interfaces;
using SEM_5.Loggers;
using SEM_5.Storages;
using SEM_5.Naive;
using SEM_5.DI;

namespace SEM_5;

class Program
{
    static void Main(string[] args)
    {
        Console.WriteLine("=== Наивная реализация ===");
        var naiveService = new NaiveBookCatalogService();
        naiveService.AddBook("Евгений Онегин", "Пушкин");
        naiveService.RemoveBook("Евгений Онегин");

        Console.WriteLine("\n=== Внедрение через конструктор ===");
        var s1 = new BookCatalogService_DI_Constructor(new ConsoleLogger());
        s1.AddBook("Евгений Онегин", "Пушкин");
        
        var s2 = new BookCatalogService_DI_Constructor(new FileLogger("log.txt"));
        s2.AddBook("Сборник стихотворений", "Пушкин");

        Console.WriteLine("\n=== Внедрение через свойство ===");
        var s3 = new BookCatalogService_DI_Property();
        s3.AddBook("Евгений Онегин", "Пушкин");
        s3.Logger = new ConsoleLogger();
        s3.AddBook("Сборник стихотворений", "Пушкин");

        Console.WriteLine("\n=== Внедрение через параметр метода ===");
        var s4 = new BookCatalogService_DI_Method();
        s4.AddBook("Евгений Онегин", "Пушкин", new ConsoleLogger());
        s4.AddBook("Сборник стихотворений", "Пушкин", new FileLogger("audit.log"));

        Console.WriteLine("\n=== Точка сборки (Pure DI) ===");
        ILogger logger = new ConsoleLogger();
        IBookStorage storage = new InMemoryBookStorage(logger);
        var s5 = new BookCatalogService(logger, storage);
        s5.AddBook("Евгений Онегин", "Пушкин");
        s5.AddBook("Сборник стихотворений", "Пушкин");

        Console.WriteLine("\n=== Точка сборки с фреймворком ===");
        var services = new ServiceCollection();
        services.AddSingleton<ILogger, ConsoleLogger>();
        services.AddSingleton<IBookStorage, InMemoryBookStorage>();
        services.AddTransient<BookCatalogService>();
        
        var provider = services.BuildServiceProvider(new ServiceProviderOptions { ValidateOnBuild = true });
        var s7 = provider.GetRequiredService<BookCatalogService>();
        s7.AddBook("Евгений Онегин", "Пушкин");

        Console.WriteLine("\n=== Демонстрация Scoped ===");
        var scopedServices = new ServiceCollection();
        scopedServices.AddSingleton<ILogger, ConsoleLogger>();
        scopedServices.AddScoped<IBookStorage, InMemoryBookStorage>();
        
        var scopedProvider = scopedServices.BuildServiceProvider(new ServiceProviderOptions { ValidateOnBuild = true });
        
        using (var scope1 = scopedProvider.CreateScope())
        {
            var storage1 = scope1.ServiceProvider.GetRequiredService<IBookStorage>();
            var storage2 = scope1.ServiceProvider.GetRequiredService<IBookStorage>();
            Console.WriteLine($"В рамках одной области это один объект: {object.ReferenceEquals(storage1, storage2)}");
        }

        using (var scope2 = scopedProvider.CreateScope())
        {
            var storage3 = scope2.ServiceProvider.GetRequiredService<IBookStorage>();
        }

        Console.WriteLine("\n=== Решение контрольного задания ===");
        var ctServices = new ServiceCollection();
        ctServices.AddSingleton<ILogger, ConsoleLogger>();
        ctServices.AddSingleton<IBookStorage, InMemoryBookStorage>();
        ctServices.AddTransient<SEM_5.ControlTask.BookCatalogService>();
        
        var ctProvider = ctServices.BuildServiceProvider(new ServiceProviderOptions { ValidateOnBuild = true });
        var ctService = ctProvider.GetRequiredService<SEM_5.ControlTask.BookCatalogService>();
        ctService.AddBook("Евгений Онегин", "Пушкин");
        ctService.RemoveBook("Евгений Онегин");
    }
}
