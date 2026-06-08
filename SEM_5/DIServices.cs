using SEM_5.Interfaces;

namespace SEM_5.DI;

public class BookCatalogService_DI_Constructor
{
    private readonly ILogger _logger;

    public BookCatalogService_DI_Constructor(ILogger logger)
    {
        _logger = logger;
    }

    public void AddBook(string title, string author)
    {
        _logger.Log($"Добавлена книга: «{title}» — {author}");
    }

    public void RemoveBook(string title)
    {
        _logger.Log($"Удалена книга: «{title}»");
    }
}

public class BookCatalogService_DI_Property
{
    public ILogger Logger { get; set; } = new Loggers.NullLogger();

    public void AddBook(string title, string author)
    {
        Logger.Log($"Добавлена книга: «{title}» — {author}");
    }

    public void RemoveBook(string title)
    {
        Logger.Log($"Удалена книга: «{title}»");
    }
}

public class BookCatalogService_DI_Method
{
    public void AddBook(string title, string author, ILogger logger)
    {
        logger.Log($"Добавлена книга: «{title}» — {author}");
    }
}

public class BookCatalogService
{
    private readonly ILogger _logger;
    private readonly IBookStorage _storage;

    public BookCatalogService(ILogger logger, IBookStorage storage)
    {
        _logger = logger;
        _storage = storage;
    }

    public void AddBook(string title, string author)
    {
        _storage.Save(title, author);
        _logger.Log($"Добавлена книга: «{title}» — {author}");
    }

    public void RemoveBook(string title)
    {
        _logger.Log($"Удалена книга: «{title}»");
    }
}
