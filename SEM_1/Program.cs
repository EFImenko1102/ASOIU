using System;
using System.Globalization;

Console.WriteLine("=== АНАЛИЗ КВАЛИФИКАЦИИ ГРАН-ПРИ ===");
Console.WriteLine();

int n = ReadPositiveInteger("Введите количество участников: ");
Console.WriteLine();

string[] teams = new string[n];
double[] avgSpeeds = new double[n];

InputData(teams, avgSpeeds, n);

Console.WriteLine("--- СТАТИСТИКА КВАЛИФИКАЦИИ ---");
CalculateStatistics(teams, avgSpeeds, n);
Console.WriteLine();

Console.WriteLine("--- ИСХОДНЫЙ ПОРЯДОК ---");
PrintTable(teams, avgSpeeds, n, false);
Console.WriteLine();

string[] sortedTeams = new string[n];
double[] sortedSpeeds = new double[n];
CopyArrays(teams, avgSpeeds, sortedTeams, sortedSpeeds, n);

SelectionSort(sortedTeams, sortedSpeeds, n);

Console.WriteLine("--- ИТОГОВЫЙ ПРОТОКОЛ КВАЛИФИКАЦИИ ---");
PrintTable(sortedTeams, sortedSpeeds, n, true);
Console.WriteLine();

FilterBySpeed(sortedTeams, sortedSpeeds, n);

Console.WriteLine();
Console.Write("Нажмите любую клавишу для выхода...");

if (!Console.IsInputRedirected)
{
    Console.ReadKey();
}

static int ReadPositiveInteger(string prompt)
{
    int value;
    while (true)
    {
        Console.Write(prompt);
        string input = Console.ReadLine() ?? "";
        if (int.TryParse(input, out value) && value > 0)
        {
            return value;
        }
        Console.WriteLine("Ошибка: введите целое положительное число.");
    }
}

static double ReadDouble(string prompt)
{
    double value;
    while (true)
    {
        Console.Write(prompt);
        string input = Console.ReadLine() ?? "";
        input = input.Replace(',', '.');
        if (double.TryParse(input, NumberStyles.Any, CultureInfo.InvariantCulture, out value) && value >= 0)
        {
            return value;
        }
        Console.WriteLine("Ошибка: введите корректное числовое значение (например, 228.5).");
    }
}

static void InputData(string[] teams, double[] speeds, int n)
{
    for (int i = 0; i < n; i++)
    {
        Console.WriteLine($"Участник #{i + 1}");
        Console.Write("Команда: ");
        teams[i] = Console.ReadLine() ?? "Без названия";
        speeds[i] = ReadDouble("Средняя скорость (км/ч): ");
        Console.WriteLine();
    }
}

static void CalculateStatistics(string[] teams, double[] speeds, int n)
{
    double sum = 0;
    for (int i = 0; i < n; i++)
    {
        sum += speeds[i];
    }
    double average = sum / n;

    double maxSpeed = speeds[0];
    double minSpeed = speeds[0];
    string fastestTeam = teams[0];
    string slowestTeam = teams[0];

    for (int i = 1; i < n; i++)
    {
        if (speeds[i] > maxSpeed)
        {
            maxSpeed = speeds[i];
            fastestTeam = teams[i];
        }
        if (speeds[i] < minSpeed)
        {
            minSpeed = speeds[i];
            slowestTeam = teams[i];
        }
    }

    Console.WriteLine($"Средняя скорость: {average:F2} км/ч");
    Console.WriteLine($"Лидер: {fastestTeam} ({maxSpeed:F2} км/ч)");
    Console.WriteLine($"Самый медленный: {slowestTeam} ({minSpeed:F2} км/ч)");
    Console.WriteLine($"Разница темпа: {maxSpeed - minSpeed:F2} км/ч");
}

static void PrintTable(string[] teams, double[] speeds, int n, bool showPosition)
{
    Console.WriteLine("-----------------------------------------------");
    if (showPosition)
    {
        Console.WriteLine("| Поз. | Команда             | Скорость      |");
    }
    else
    {
        Console.WriteLine("| Команда             | Скорость (км/ч)      |");
    }
    Console.WriteLine("-----------------------------------------------");
    for (int i = 0; i < n; i++)
    {
        if (showPosition)
        {
            Console.WriteLine($"| {i + 1,4} | {teams[i],-20} | {speeds[i],13:F2} |");
        }
        else
        {
            Console.WriteLine($"| {teams[i],-20} | {speeds[i],19:F2} |");
        }
    }
    Console.WriteLine("-----------------------------------------------");
}

static void SelectionSort(string[] teams, double[] speeds, int n)
{
    for (int i = 0; i < n - 1; i++)
    {
        int maxIndex = i;
        for (int j = i + 1; j < n; j++)
        {
            if (speeds[j] > speeds[maxIndex])
            {
                maxIndex = j;
            }
        }
        if (maxIndex != i)
        {
            (speeds[i], speeds[maxIndex]) = (speeds[maxIndex], speeds[i]);
            (teams[i], teams[maxIndex]) = (teams[maxIndex], teams[i]);
        }
    }
}

static void CopyArrays(string[] srcTeams, double[] srcSpeeds,
                      string[] dstTeams, double[] dstSpeeds, int n)
{
    for (int i = 0; i < n; i++)
    {
        dstTeams[i] = srcTeams[i];
        dstSpeeds[i] = srcSpeeds[i];
    }
}

static void FilterBySpeed(string[] teams, double[] speeds, int n)
{
    Console.WriteLine("--- ДОПОЛНИТЕЛЬНО: ФИЛЬТР ПО СКОРОСТИ ---");
    double minSpeed = ReadDouble("Введите минимальную скорость для отбора (км/ч): ");
    Console.WriteLine();

    Console.WriteLine($"Команды со скоростью >= {minSpeed:F2} км/ч:");
    int count = 0;
    for (int i = 0; i < n; i++)
    {
        if (speeds[i] >= minSpeed)
        {
            Console.WriteLine($"- {teams[i]} ({speeds[i]:F2} км/ч)");
            count++;
        }
    }
    Console.WriteLine();
    Console.WriteLine($"Отобрано команд: {count}");
}
