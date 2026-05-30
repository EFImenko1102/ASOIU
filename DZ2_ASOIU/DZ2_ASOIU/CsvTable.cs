namespace DZ2_ASOIU;

/// <summary>
/// Одна строка табличного результата. Хранит значения полей в порядке столбцов.
/// </summary>
record CsvRow(string[] Fields);

/// <summary>
/// Табличный результат: заголовки столбцов и список строк.
/// </summary>
record CsvTable(string[] Headers, List<CsvRow> Rows);
