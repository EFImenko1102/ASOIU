namespace SEM_6;

public abstract class LibraryItem
{
    public string Title { get; set; } = string.Empty;

    public abstract string GetDisplayInfo();
}

public class Book : LibraryItem
{
    public string Author { get; set; } = string.Empty;
    public int Year { get; set; }

    public override string GetDisplayInfo()
    {
        return $"Книга: {Title}";
    }
}

public class Magazine : LibraryItem
{
    public int IssueNumber { get; set; }

    public override string GetDisplayInfo()
    {
        return $"Журнал: {Title}";
    }
}
