using System;
using System.Collections.Generic;

namespace SEM_6;

public interface IReportPrinter
{
    void Print(IEnumerable<LibraryItem> items);
}

public class LibraryReportPrinter : IReportPrinter
{
    public void Print(IEnumerable<LibraryItem> items)
    {
        int count = 0;
        foreach (var _ in items)
        {
            count++;
        }

        Console.WriteLine($"=== Отчёт: {count} элементов ===");
        foreach (var item in items)
        {
            Console.WriteLine(item.GetDisplayInfo());
        }
    }
}
