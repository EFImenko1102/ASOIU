using System.IO;
using SEM_5.Interfaces;

namespace SEM_5.Loggers;

public class ConsoleLogger : ILogger
{
    public void Log(string message)
    {
        Console.WriteLine($"[LOG] {message}");
    }
}

public class FileLogger : ILogger
{
    private readonly string _filePath;

    public FileLogger(string filePath)
    {
        _filePath = filePath;
    }

    public void Log(string message)
    {
        File.AppendAllText(_filePath, $"[LOG] {message}\n");
    }
}

public class NullLogger : ILogger
{
    public void Log(string message)
    {
    }
}
