using System;
using System.IO;

namespace SEM_5.Naive;

public class NaiveConsoleLogger
{
    public void Log(string message)
    {
        Console.WriteLine($"[LOG] {message}");
    }
}

public class NaiveBookCatalogService
{
    private readonly NaiveConsoleLogger _logger = new();

    public void AddBook(string title, string author)
    {
        _logger.Log($"Добавлена книга: «{title}» — {author}");
    }

    public void RemoveBook(string title)
    {
        _logger.Log($"Удалена книга: «{title}»");
    }
}

public class NaiveFileLogger
{
    private readonly string _filePath;

    public NaiveFileLogger(string filePath)
    {
        _filePath = filePath;
    }

    public void Log(string message)
    {
        File.AppendAllText(_filePath, $"[LOG] {message}\n");
    }
}

public class NaiveBookCatalogServiceFile
{
    private readonly NaiveFileLogger _logger = new("catalog.log");

    public void AddBook(string title, string author)
    {
        _logger.Log($"Добавлена книга: «{title}» — {author}");
    }

    public void RemoveBook(string title)
    {
        _logger.Log($"Удалена книга: «{title}»");
    }
}

public class NaiveBookCatalogServiceLogFlag
{
    private readonly bool _useFile;

    public NaiveBookCatalogServiceLogFlag(bool useFile = false)
    {
        _useFile = useFile;
    }

    public void AddBook(string title, string author)
    {
        if (_useFile)
        {
            File.AppendAllText("log.txt", $"Добавлена книга: «{title}»\n");
        }
        else
        {
            Console.WriteLine($"[LOG] Добавлена книга: «{title}»");
        }
    }
}
