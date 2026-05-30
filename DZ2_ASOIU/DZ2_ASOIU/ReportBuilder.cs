using System.Text;

namespace DZ2_ASOIU;

/// <summary>
/// Строит текстовые отчеты по SQL-запросу с использованием Fluent Interface.
/// </summary>
class ReportBuilder
{
    private readonly DatabaseManager _db;
    private string _sql = "";
    private string _title = "";
    private string[] _headers = Array.Empty<string>();
    private int[] _widths = Array.Empty<int>();
    private bool _numbered;
    private string _footer = "";

    /// <summary>Создает построитель отчетов.</summary>
    public ReportBuilder(DatabaseManager db)
    {
        _db = db;
    }

    /// <summary>Задает SQL-запрос для отчета.</summary>
    public ReportBuilder Query(string sql)
    {
        _sql = sql;
        return this;
    }

    /// <summary>Задает заголовок отчета.</summary>
    public ReportBuilder Title(string title)
    {
        _title = title;
        return this;
    }

    /// <summary>Задает отображаемые заголовки колонок.</summary>
    public ReportBuilder Header(params string[] columns)
    {
        _headers = columns;
        return this;
    }

    /// <summary>Задает ширины колонок в символах.</summary>
    public ReportBuilder ColumnWidths(params int[] widths)
    {
        _widths = widths;
        return this;
    }

    /// <summary>Добавляет нумерацию строк слева.</summary>
    public ReportBuilder Numbered()
    {
        _numbered = true;
        return this;
    }

    /// <summary>Добавляет итоговую строку с количеством строк отчета.</summary>
    public ReportBuilder Footer(string label)
    {
        _footer = label;
        return this;
    }

    /// <summary>Формирует отчет и возвращает его как строку.</summary>
    public string Build()
    {
        if (string.IsNullOrWhiteSpace(_sql))
            throw new InvalidOperationException("SQL-запрос отчета не задан");

        CsvTable table = _db.ExecuteQuery(_sql);
        string[] displayHeaders = _headers.Length > 0 ? _headers : table.Headers;
        int colCount = displayHeaders.Length;
        int[] widths = GetWidths(colCount);
        int numWidth = _numbered ? 5 : 0;
        int totalWidth = numWidth;

        for (int i = 0; i < colCount; i++)
            totalWidth += widths[i];

        var sb = new StringBuilder();
        if (_title.Length > 0)
        {
            sb.AppendLine();
            sb.AppendLine($"=== {_title} ===");
        }

        if (_numbered)
            sb.Append("№".PadRight(numWidth));

        for (int i = 0; i < colCount; i++)
            sb.Append(displayHeaders[i].PadRight(widths[i]));

        sb.AppendLine();
        sb.AppendLine(new string('-', totalWidth));

        for (int r = 0; r < table.Rows.Count; r++)
        {
            if (_numbered)
                sb.Append((r + 1).ToString().PadRight(numWidth));

            for (int c = 0; c < table.Rows[r].Fields.Length && c < colCount; c++)
                sb.Append(TrimToWidth(table.Rows[r].Fields[c], widths[c]).PadRight(widths[c]));

            sb.AppendLine();
        }

        if (_footer.Length > 0)
        {
            sb.AppendLine(new string('-', totalWidth));
            sb.AppendLine($"{_footer}: {table.Rows.Count}");
        }

        return sb.ToString();
    }

    /// <summary>Формирует отчет и выводит его в консоль.</summary>
    public void Print()
    {
        Console.Write(Build());
    }

    /// <summary>Формирует отчет и сохраняет его в текстовый файл.</summary>
    public void SaveToFile(string path)
    {
        File.WriteAllText(path, Build());
        Console.WriteLine($"Отчет сохранен в файл: {path}");
    }

    private int[] GetWidths(int colCount)
    {
        if (_widths.Length >= colCount)
            return _widths;

        var result = new int[colCount];
        for (int i = 0; i < colCount; i++)
            result[i] = 20;

        return result;
    }

    private static string TrimToWidth(string value, int width)
    {
        if (value.Length <= width)
            return value;

        return value[..Math.Max(0, width - 1)] + "~";
    }
}
