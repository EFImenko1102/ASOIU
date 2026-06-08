using System;
using System.Collections.Generic;
using System.IO;

namespace SEM_6;

public interface IReportExporter
{
    void Export(IEnumerable<LibraryItem> items, string reportName);
}

public class FileReportExporter : IReportExporter
{
    public void Export(IEnumerable<LibraryItem> items, string reportName)
    {
        int count = 0;
        foreach (var _ in items)
        {
            count++;
        }
        File.WriteAllText($"{reportName}.txt", $"Всего элементов: {count}\nДата: {DateTime.Now:u}");
    }
}
