using System.Text;

namespace DZ2_ASOIU;

class Program
{
    static void Main(string[] args)
    {
        Console.OutputEncoding = Encoding.UTF8;
        Console.InputEncoding = Encoding.UTF8;

        string dbPath = Path.Combine(AppContext.BaseDirectory, "cities.db");
        string countryCsv = Path.Combine(AppContext.BaseDirectory, "country.csv");
        string cityCsv = Path.Combine(AppContext.BaseDirectory, "city.csv");

        var db = new DatabaseManager(dbPath);
        db.InitializeDatabase(countryCsv, cityCsv);

        string choice;
        do
        {
            Console.WriteLine("╔══════════════════════════════════════╗");
            Console.WriteLine("║          СТРАНЫ И ГОРОДА             ║");
            Console.WriteLine("╠══════════════════════════════════════╣");
            Console.WriteLine("║ 1 - Показать все страны              ║");
            Console.WriteLine("║ 2 - Показать все города              ║");
            Console.WriteLine("║ 3 - Добавить город                   ║");
            Console.WriteLine("║ 4 - Редактировать город              ║");
            Console.WriteLine("║ 5 - Удалить город                    ║");
            Console.WriteLine("║ 6 - Отчеты                           ║");
            Console.WriteLine("║ 7 - Фильтр по стране                 ║");
            Console.WriteLine("║ 8 - Экспорт в CSV                    ║");
            Console.WriteLine("║ 9 - Добавить страну                  ║");
            Console.WriteLine("║ 10 - Удалить страну                  ║");
            Console.WriteLine("║ 0 - Выход                            ║");
            Console.WriteLine("╚══════════════════════════════════════╝");
            Console.Write("Ваш выбор: ");

            choice = Console.ReadLine()?.Trim() ?? "";
            Console.WriteLine();

            switch (choice)
            {
                case "1": ShowCountries(db); break;
                case "2": ShowCities(db); break;
                case "3": AddCity(db); break;
                case "4": EditCity(db); break;
                case "5": DeleteCity(db); break;
                case "6": ReportsMenu(db); break;
                case "7": FilterByCountry(db); break;
                case "8": ExportCsv(db); break;
                case "9": AddCountry(db); break;
                case "10": DeleteCountry(db); break;
                case "0": Console.WriteLine("До свидания!"); break;
                default: Console.WriteLine("Неверный пункт меню."); break;
            }

            Console.WriteLine();
        }
        while (choice != "0");
    }

    static void ShowCountries(DatabaseManager db)
    {
        Console.WriteLine("--- Все страны ---");
        var countries = db.GetAllCountries();
        foreach (var country in countries)
            Console.WriteLine(" " + country);
        Console.WriteLine($"Итого: {countries.Count}");
    }

    static void AddCountry(DatabaseManager db)
    {
        Console.WriteLine("--- Добавление страны ---");
        Console.Write("Название страны: ");
        string name = Console.ReadLine()?.Trim() ?? "";
        if (name.Length == 0)
        {
            Console.WriteLine("Ошибка: название не может быть пустым.");
            return;
        }

        try
        {
            var country = new Country(0, name);
            db.AddCountry(country);
            Console.WriteLine("Страна добавлена.");
        }
        catch (Microsoft.Data.Sqlite.SqliteException ex)
        {
            Console.WriteLine($"Ошибка базы данных: {ex.Message}");
        }
    }

    static void DeleteCountry(DatabaseManager db)
    {
        Console.WriteLine("--- Удаление страны ---");
        if (!ReadInt("Введите ID страны: ", out int id))
            return;

        var country = db.GetCountryById(id);
        if (country == null)
        {
            Console.WriteLine($"Страна с ID={id} не найдена.");
            return;
        }

        Console.Write($"Удалить страну '{country.Name}'? (да/нет): ");
        string confirm = Console.ReadLine()?.Trim().ToLowerInvariant() ?? "";
        if (confirm == "да")
        {
            try
            {
                db.DeleteCountry(id);
                Console.WriteLine("Страна удалена.");
            }
            catch (Microsoft.Data.Sqlite.SqliteException ex)
            {
                Console.WriteLine($"Ошибка при удалении: {ex.Message}");
                Console.WriteLine("Возможно, в этой стране есть города. Сначала удалите города этой страны.");
            }
        }
        else
        {
            Console.WriteLine("Удаление отменено.");
        }
    }

    static void ShowCities(DatabaseManager db)
    {
        Console.WriteLine("--- Все города ---");
        var cities = db.GetAllCities();
        foreach (var city in cities)
            Console.WriteLine(" " + city);
        Console.WriteLine($"Итого: {cities.Count}");
    }

    static void AddCity(DatabaseManager db)
    {
        Console.WriteLine("--- Добавление города ---");
        ShowCountryList(db);

        if (!ReadInt("ID страны: ", out int countryId))
            return;

        Console.Write("Название города: ");
        string name = Console.ReadLine()?.Trim() ?? "";
        if (name.Length == 0)
        {
            Console.WriteLine("Ошибка: название не может быть пустым.");
            return;
        }

        if (!ReadInt("Население, тыс. чел.: ", out int populationK))
            return;

        try
        {
            var city = new City(0, countryId, name, populationK);
            db.AddCity(city);
            Console.WriteLine("Город добавлен.");
        }
        catch (ArgumentException ex)
        {
            Console.WriteLine($"Ошибка: {ex.Message}");
        }
        catch (Microsoft.Data.Sqlite.SqliteException ex)
        {
            Console.WriteLine($"Ошибка базы данных: {ex.Message}");
        }
    }

    static void EditCity(DatabaseManager db)
    {
        Console.WriteLine("--- Редактирование города ---");
        if (!ReadInt("Введите ID города: ", out int id))
            return;

        var city = db.GetCityById(id);
        if (city == null)
        {
            Console.WriteLine($"Город с ID={id} не найден.");
            return;
        }

        Console.WriteLine($"Текущие данные: {city}");
        Console.WriteLine("Нажмите Enter, чтобы оставить значение без изменений.");

        Console.Write($"Название [{city.Name}]: ");
        string input = Console.ReadLine()?.Trim() ?? "";
        if (input.Length > 0)
            city.Name = input;

        ShowCountryList(db);
        Console.Write($"ID страны [{city.CountryId}]: ");
        input = Console.ReadLine()?.Trim() ?? "";
        if (input.Length > 0)
        {
            if (!int.TryParse(input, out int countryId))
            {
                Console.WriteLine("Ошибка: введите целое число.");
                return;
            }

            city.CountryId = countryId;
        }

        Console.Write($"Население, тыс. чел. [{city.PopulationK}]: ");
        input = Console.ReadLine()?.Trim() ?? "";
        if (input.Length > 0)
        {
            if (!int.TryParse(input, out int populationK))
            {
                Console.WriteLine("Ошибка: введите целое число.");
                return;
            }

            try
            {
                city.PopulationK = populationK;
            }
            catch (ArgumentException ex)
            {
                Console.WriteLine($"Ошибка: {ex.Message}");
                return;
            }
        }

        try
        {
            db.UpdateCity(city);
            Console.WriteLine("Данные обновлены.");
        }
        catch (Microsoft.Data.Sqlite.SqliteException ex)
        {
            Console.WriteLine($"Ошибка базы данных: {ex.Message}");
        }
    }

    static void DeleteCity(DatabaseManager db)
    {
        Console.WriteLine("--- Удаление города ---");
        if (!ReadInt("Введите ID города: ", out int id))
            return;

        var city = db.GetCityById(id);
        if (city == null)
        {
            Console.WriteLine($"Город с ID={id} не найден.");
            return;
        }

        Console.Write($"Удалить '{city.Name}'? (да/нет): ");
        string confirm = Console.ReadLine()?.Trim().ToLowerInvariant() ?? "";
        if (confirm == "да")
        {
            db.DeleteCity(id);
            Console.WriteLine("Город удален.");
        }
        else
        {
            Console.WriteLine("Удаление отменено.");
        }
    }

    static void ReportsMenu(DatabaseManager db)
    {
        string choice;
        do
        {
            Console.WriteLine("--- Отчеты ---");
            Console.WriteLine(" 1 - Города по странам");
            Console.WriteLine(" 2 - Количество городов по странам");
            Console.WriteLine(" 3 - Среднее население городов по странам");
            Console.WriteLine(" 4 - Сохранить отчет 1 в файл");
            Console.WriteLine(" 0 - Назад");
            Console.Write("Ваш выбор: ");

            choice = Console.ReadLine()?.Trim() ?? "";
            Console.WriteLine();

            switch (choice)
            {
                case "1": ReportCitiesWithCountries(db); break;
                case "2": ReportCountByCountry(db); break;
                case "3": ReportAvgPopulationByCountry(db); break;
                case "4": SaveReportToFile(db); break;
                case "0": break;
                default: Console.WriteLine("Неверный пункт."); break;
            }

            Console.WriteLine();
        }
        while (choice != "0");
    }

    static void ReportCitiesWithCountries(DatabaseManager db)
    {
        new ReportBuilder(db)
            .Query(@"SELECT city.city_name, country.country_name, city.population_k
                     FROM city
                     JOIN country ON city.country_id = country.country_id
                     ORDER BY city.city_name")
            .Title("Города по странам")
            .Header("Город", "Страна", "Население, тыс.")
            .ColumnWidths(22, 18, 18)
            .Numbered()
            .Footer("Всего записей")
            .Print();
    }

    static void ReportCountByCountry(DatabaseManager db)
    {
        new ReportBuilder(db)
            .Query(@"SELECT country.country_name, COUNT(*) AS city_count
                     FROM city
                     JOIN country ON city.country_id = country.country_id
                     GROUP BY country.country_name
                     ORDER BY country.country_name")
            .Title("Количество городов по странам")
            .Header("Страна", "Кол-во городов")
            .ColumnWidths(20, 18)
            .Print();
    }

    static void ReportAvgPopulationByCountry(DatabaseManager db)
    {
        new ReportBuilder(db)
            .Query(@"SELECT country.country_name, ROUND(AVG(city.population_k), 1) AS avg_population
                     FROM city
                     JOIN country ON city.country_id = country.country_id
                     GROUP BY country.country_name
                     ORDER BY avg_population DESC")
            .Title("Среднее население городов по странам")
            .Header("Страна", "Среднее, тыс.")
            .ColumnWidths(20, 18)
            .Print();
    }

    static void FilterByCountry(DatabaseManager db)
    {
        Console.WriteLine("--- Фильтр по стране ---");
        ShowCountryList(db);

        if (!ReadInt("Введите ID страны: ", out int countryId))
            return;

        var cities = db.GetCitiesByCountry(countryId);
        if (cities.Count == 0)
        {
            Console.WriteLine("Для выбранной страны города не найдены.");
            return;
        }

        foreach (var city in cities)
            Console.WriteLine(" " + city);
        Console.WriteLine($"Итого: {cities.Count}");
    }

    static void ExportCsv(DatabaseManager db)
    {
        string countryPath = Path.Combine(AppContext.BaseDirectory, "country_export.csv");
        string cityPath = Path.Combine(AppContext.BaseDirectory, "city_export.csv");

        db.ExportToCsv(countryPath, cityPath);
        Console.WriteLine($"Страны экспортированы в: {countryPath}");
        Console.WriteLine($"Города экспортированы в: {cityPath}");
    }

    static void SaveReportToFile(DatabaseManager db)
    {
        string reportPath = Path.Combine(AppContext.BaseDirectory, "cities_report.txt");
        new ReportBuilder(db)
            .Query(@"SELECT city.city_name, country.country_name, city.population_k
                     FROM city
                     JOIN country ON city.country_id = country.country_id
                     ORDER BY city.city_name")
            .Title("Города по странам")
            .Header("Город", "Страна", "Население, тыс.")
            .ColumnWidths(22, 18, 18)
            .Numbered()
            .Footer("Всего записей")
            .SaveToFile(reportPath);
    }

    static void ShowCountryList(DatabaseManager db)
    {
        Console.WriteLine("Доступные страны:");
        foreach (var country in db.GetAllCountries())
            Console.WriteLine(" " + country);
    }

    static bool ReadInt(string prompt, out int value)
    {
        Console.Write(prompt);
        if (int.TryParse(Console.ReadLine(), out value))
            return true;

        Console.WriteLine("Ошибка: введите целое число.");
        return false;
    }
}
