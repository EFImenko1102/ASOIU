using System;

namespace SEM_6;

class Program
{
    static void Main(string[] args)
    {
        var validator = new LibraryItemValidator();
        var logger = new FileLogger();
        var printer = new LibraryReportPrinter();
        var exporter = new FileReportExporter();

        var libraryService = new LibraryService(validator, logger, printer, exporter);

        try
        {
            libraryService.AddBook("Война и мир", "Л.Н. Толстой", 1869);
            libraryService.AddMagazine("Наука и жизнь", 5);
            libraryService.PrintReport();
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Ошибка: {ex.Message}");
        }
    }
}
