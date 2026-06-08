using System;
using System.IO;

namespace SEM_6;

public interface ILogger
{
    void Log(string message);
}

public class FileLogger : ILogger
{
    private readonly string _filePath;

    public FileLogger(string filePath = "library.log")
    {
        _filePath = filePath;
    }

    public void Log(string message)
    {
        File.AppendAllText(_filePath, $"{DateTime.Now:u}: {message}\n");
    }
}
