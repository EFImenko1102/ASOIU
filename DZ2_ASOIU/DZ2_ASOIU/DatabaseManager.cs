using Microsoft.Data.Sqlite;

namespace DZ2_ASOIU;

/// <summary>
/// Управляет базой данных SQLite: создает таблицы, импортирует CSV,
/// выполняет CRUD-операции и запросы для отчетов.
/// </summary>
class DatabaseManager
{
    private readonly string _connectionString;

    /// <summary>Создает менеджер базы данных по пути к файлу SQLite.</summary>
    public DatabaseManager(string dbPath)
    {
        _connectionString = $"Data Source={dbPath};Foreign Keys=True";
        CreateTables();
    }

    /// <summary>Инициализирует таблицы и загружает CSV, если таблицы пусты.</summary>
    public void InitializeDatabase(string countryCsvPath, string cityCsvPath)
    {
        if (GetAllCountries().Count == 0 && File.Exists(countryCsvPath))
        {
            ImportCountriesFromCsv(countryCsvPath);
            Console.WriteLine($"[OK] Загружены страны из {countryCsvPath}");
        }

        if (GetAllCities().Count == 0 && File.Exists(cityCsvPath))
        {
            ImportCitiesFromCsv(cityCsvPath);
            Console.WriteLine($"[OK] Загружены города из {cityCsvPath}");
        }
    }

    /// <summary>Импортирует данные из двух CSV-файлов.</summary>
    public void ImportFromCsv(string countryCsvPath, string cityCsvPath)
    {
        ImportCountriesFromCsv(countryCsvPath);
        ImportCitiesFromCsv(cityCsvPath);
    }

    /// <summary>Возвращает все страны.</summary>
    public List<Country> GetAllCountries()
    {
        var result = new List<Country>();

        using var connection = new SqliteConnection(_connectionString);
        connection.Open();

        var cmd = connection.CreateCommand();
        cmd.CommandText = "SELECT country_id, country_name FROM country ORDER BY country_id";

        using var reader = cmd.ExecuteReader();
        while (reader.Read())
            result.Add(new Country(reader.GetInt32(0), reader.GetString(1)));

        return result;
    }

    /// <summary>Возвращает страну по идентификатору или null, если страна не найдена.</summary>
    public Country? GetCountryById(int id)
    {
        using var connection = new SqliteConnection(_connectionString);
        connection.Open();

        var cmd = connection.CreateCommand();
        cmd.CommandText = @"
            SELECT country_id, country_name
            FROM country
            WHERE country_id = $id";
        cmd.Parameters.AddWithValue("$id", id);

        using var reader = cmd.ExecuteReader();
        if (!reader.Read())
            return null;

        return new Country(reader.GetInt32(0), reader.GetString(1));
    }

    /// <summary>Добавляет страну.</summary>
    public void AddCountry(Country country)
    {
        using var connection = new SqliteConnection(_connectionString);
        connection.Open();

        var cmd = connection.CreateCommand();
        cmd.CommandText = @"
            INSERT INTO country (country_name)
            VALUES ($name)";
        cmd.Parameters.AddWithValue("$name", country.Name);
        cmd.ExecuteNonQuery();
    }

    /// <summary>Удаляет страну по идентификатору.</summary>
    public void DeleteCountry(int id)
    {
        using var connection = new SqliteConnection(_connectionString);
        connection.Open();

        var cmd = connection.CreateCommand();
        cmd.CommandText = "DELETE FROM country WHERE country_id = $id";
        cmd.Parameters.AddWithValue("$id", id);
        cmd.ExecuteNonQuery();
    }

    /// <summary>Возвращает все города.</summary>
    public List<City> GetAllCities()
    {
        var result = new List<City>();

        using var connection = new SqliteConnection(_connectionString);
        connection.Open();

        var cmd = connection.CreateCommand();
        cmd.CommandText = "SELECT city_id, country_id, city_name, population_k FROM city ORDER BY city_id";

        using var reader = cmd.ExecuteReader();
        while (reader.Read())
        {
            result.Add(new City(
                reader.GetInt32(0),
                reader.GetInt32(1),
                reader.GetString(2),
                reader.GetInt32(3)));
        }

        return result;
    }

    /// <summary>Возвращает город по идентификатору или null, если город не найден.</summary>
    public City? GetCityById(int id)
    {
        using var connection = new SqliteConnection(_connectionString);
        connection.Open();

        var cmd = connection.CreateCommand();
        cmd.CommandText = @"
            SELECT city_id, country_id, city_name, population_k
            FROM city
            WHERE city_id = $id";
        cmd.Parameters.AddWithValue("$id", id);

        using var reader = cmd.ExecuteReader();
        if (!reader.Read())
            return null;

        return new City(
            reader.GetInt32(0),
            reader.GetInt32(1),
            reader.GetString(2),
            reader.GetInt32(3));
    }

    /// <summary>Добавляет город.</summary>
    public void AddCity(City city)
    {
        using var connection = new SqliteConnection(_connectionString);
        connection.Open();

        var cmd = connection.CreateCommand();
        cmd.CommandText = @"
            INSERT INTO city (country_id, city_name, population_k)
            VALUES ($countryId, $name, $populationK)";
        cmd.Parameters.AddWithValue("$countryId", city.CountryId);
        cmd.Parameters.AddWithValue("$name", city.Name);
        cmd.Parameters.AddWithValue("$populationK", city.PopulationK);
        cmd.ExecuteNonQuery();
    }

    /// <summary>Обновляет существующий город.</summary>
    public void UpdateCity(City city)
    {
        using var connection = new SqliteConnection(_connectionString);
        connection.Open();

        var cmd = connection.CreateCommand();
        cmd.CommandText = @"
            UPDATE city
            SET country_id = $countryId,
                city_name = $name,
                population_k = $populationK
            WHERE city_id = $id";
        cmd.Parameters.AddWithValue("$id", city.Id);
        cmd.Parameters.AddWithValue("$countryId", city.CountryId);
        cmd.Parameters.AddWithValue("$name", city.Name);
        cmd.Parameters.AddWithValue("$populationK", city.PopulationK);
        cmd.ExecuteNonQuery();
    }

    /// <summary>Удаляет город по идентификатору.</summary>
    public void DeleteCity(int id)
    {
        using var connection = new SqliteConnection(_connectionString);
        connection.Open();

        var cmd = connection.CreateCommand();
        cmd.CommandText = "DELETE FROM city WHERE city_id = $id";
        cmd.Parameters.AddWithValue("$id", id);
        cmd.ExecuteNonQuery();
    }

    /// <summary>Возвращает города выбранной страны.</summary>
    public List<City> GetCitiesByCountry(int countryId)
    {
        var result = new List<City>();

        using var connection = new SqliteConnection(_connectionString);
        connection.Open();

        var cmd = connection.CreateCommand();
        cmd.CommandText = @"
            SELECT city_id, country_id, city_name, population_k
            FROM city
            WHERE country_id = $countryId
            ORDER BY city_name";
        cmd.Parameters.AddWithValue("$countryId", countryId);

        using var reader = cmd.ExecuteReader();
        while (reader.Read())
        {
            result.Add(new City(
                reader.GetInt32(0),
                reader.GetInt32(1),
                reader.GetString(2),
                reader.GetInt32(3)));
        }

        return result;
    }

    /// <summary>Выполняет SQL-запрос и возвращает результат для форматирования отчета.</summary>
    public CsvTable ExecuteQuery(string sql)
    {
        var rows = new List<CsvRow>();

        using var connection = new SqliteConnection(_connectionString);
        connection.Open();

        var cmd = connection.CreateCommand();
        cmd.CommandText = sql;

        using var reader = cmd.ExecuteReader();
        var headers = new string[reader.FieldCount];
        for (int i = 0; i < reader.FieldCount; i++)
            headers[i] = reader.GetName(i);

        while (reader.Read())
        {
            var fields = new string[reader.FieldCount];
            for (int i = 0; i < reader.FieldCount; i++)
                fields[i] = reader.IsDBNull(i) ? "" : reader.GetValue(i).ToString() ?? "";

            rows.Add(new CsvRow(fields));
        }

        return new CsvTable(headers, rows);
    }

    /// <summary>Экспортирует таблицы в CSV-файлы.</summary>
    public void ExportToCsv(string countryPath, string cityPath)
    {
        using (var writer = new StreamWriter(countryPath))
        {
            writer.WriteLine("country_id;country_name");
            foreach (var country in GetAllCountries())
                writer.WriteLine($"{country.Id};{country.Name}");
        }

        using (var writer = new StreamWriter(cityPath))
        {
            writer.WriteLine("city_id;country_id;city_name;population_k");
            foreach (var city in GetAllCities())
                writer.WriteLine($"{city.Id};{city.CountryId};{city.Name};{city.PopulationK}");
        }
    }

    private void CreateTables()
    {
        using var connection = new SqliteConnection(_connectionString);
        connection.Open();

        var cmd = connection.CreateCommand();
        cmd.CommandText = @"
            PRAGMA foreign_keys = ON;

            CREATE TABLE IF NOT EXISTS country (
                country_id   INTEGER PRIMARY KEY AUTOINCREMENT,
                country_name TEXT NOT NULL
            );

            CREATE TABLE IF NOT EXISTS city (
                city_id      INTEGER PRIMARY KEY AUTOINCREMENT,
                country_id   INTEGER NOT NULL,
                city_name    TEXT NOT NULL,
                population_k INTEGER NOT NULL CHECK (population_k >= 0),
                FOREIGN KEY (country_id) REFERENCES country(country_id)
            );";
        cmd.ExecuteNonQuery();
    }

    private void ImportCountriesFromCsv(string path)
    {
        using var connection = new SqliteConnection(_connectionString);
        connection.Open();

        string[] lines = File.ReadAllLines(path);
        for (int i = 1; i < lines.Length; i++)
        {
            string line = lines[i];
            if (string.IsNullOrWhiteSpace(line))
                continue;

            string[] parts = line.Split(';');
            if (parts.Length < 2 || !int.TryParse(parts[0], out int id))
                continue;

            var cmd = connection.CreateCommand();
            cmd.CommandText = @"
                INSERT OR IGNORE INTO country (country_id, country_name)
                VALUES ($id, $name)";
            cmd.Parameters.AddWithValue("$id", id);
            cmd.Parameters.AddWithValue("$name", parts[1].Trim());
            cmd.ExecuteNonQuery();
        }
    }

    private void ImportCitiesFromCsv(string path)
    {
        using var connection = new SqliteConnection(_connectionString);
        connection.Open();

        string[] lines = File.ReadAllLines(path);
        for (int i = 1; i < lines.Length; i++)
        {
            string line = lines[i];
            if (string.IsNullOrWhiteSpace(line))
                continue;

            string[] parts = line.Split(';');
            if (parts.Length < 4
                || !int.TryParse(parts[0], out int id)
                || !int.TryParse(parts[1], out int countryId)
                || !int.TryParse(parts[3], out int populationK))
            {
                continue;
            }

            var city = new City(id, countryId, parts[2].Trim(), populationK);
            var cmd = connection.CreateCommand();
            cmd.CommandText = @"
                INSERT OR IGNORE INTO city (city_id, country_id, city_name, population_k)
                VALUES ($id, $countryId, $name, $populationK)";
            cmd.Parameters.AddWithValue("$id", city.Id);
            cmd.Parameters.AddWithValue("$countryId", city.CountryId);
            cmd.Parameters.AddWithValue("$name", city.Name);
            cmd.Parameters.AddWithValue("$populationK", city.PopulationK);
            cmd.ExecuteNonQuery();
        }
    }
}
