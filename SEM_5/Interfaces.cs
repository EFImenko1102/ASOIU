namespace SEM_5.Interfaces;

public interface ILogger
{
    void Log(string message);
}

public interface IBookStorage
{
    void Save(string title, string author);
}
