using System.Collections.Generic;

namespace SEM_6;

public class LibraryService
{
    private readonly List<LibraryItem> _items = new();
    private readonly LibraryItemValidator _validator;
    private readonly ILogger _logger;
    private readonly IReportPrinter _printer;
    private readonly IReportExporter _exporter;

    public LibraryService(
        LibraryItemValidator validator,
        ILogger logger,
        IReportPrinter printer,
        IReportExporter exporter)
    {
        _validator = validator;
        _logger = logger;
        _printer = printer;
        _exporter = exporter;
    }

    public void AddBook(string title, string author, int year)
    {
        _validator.ValidateBook(title, author, year);
        var book = new Book { Title = title, Author = author, Year = year };
        _items.Add(book);
        _logger.Log($"Добавлена книга «{title}»");
    }

    public void AddMagazine(string title, int issueNumber)
    {
        _validator.ValidateMagazine(title, issueNumber);
        var magazine = new Magazine { Title = title, IssueNumber = issueNumber };
        _items.Add(magazine);
        _logger.Log($"Добавлен журнал «{title}»");
    }

    public void PrintReport()
    {
        _printer.Print(_items);
        _exporter.Export(_items, "report");
    }
}
