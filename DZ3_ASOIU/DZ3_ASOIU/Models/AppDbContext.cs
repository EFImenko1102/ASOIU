using Microsoft.EntityFrameworkCore;
using System.IO;

namespace DZ3_ASOIU.Models;
public class AppDbContext : DbContext
{
    public DbSet<Country> Countries { get; set; } = null!;
    public DbSet<City> Cities { get; set; } = null!;
    protected override void OnConfiguring(DbContextOptionsBuilder options)
    {
        string dbPath = Path.Combine(AppContext.BaseDirectory, "cities_ef.db");
        options.UseSqlite($"Data Source={dbPath}");
    }
    public void InitializeDatabase()
    {
        Database.EnsureCreated();
        if (!Countries.Any())
        {
            string countryCsvPath = Path.Combine(AppContext.BaseDirectory, "country.csv");
            if (File.Exists(countryCsvPath))
            {
                var countries = new List<Country>();
                string[] lines = File.ReadAllLines(countryCsvPath);
                for (int i = 1; i < lines.Length; i++)
                {
                    string line = lines[i];
                    if (string.IsNullOrWhiteSpace(line)) continue;

                    string[] parts = line.Split(';');
                    if (parts.Length < 2 || !int.TryParse(parts[0], out int id)) continue;

                    countries.Add(new Country { Id = id, Name = parts[1].Trim() });
                }
                Countries.AddRange(countries);
                SaveChanges();
            }
        }
        if (!Cities.Any())
        {
            string cityCsvPath = Path.Combine(AppContext.BaseDirectory, "city.csv");
            if (File.Exists(cityCsvPath))
            {
                var cities = new List<City>();
                string[] lines = File.ReadAllLines(cityCsvPath);
                for (int i = 1; i < lines.Length; i++)
                {
                    string line = lines[i];
                    if (string.IsNullOrWhiteSpace(line)) continue;

                    string[] parts = line.Split(';');
                    if (parts.Length < 4
                        || !int.TryParse(parts[0], out int id)
                        || !int.TryParse(parts[1], out int countryId)
                        || !int.TryParse(parts[3], out int populationK))
                    {
                        continue;
                    }

                    cities.Add(new City
                    {
                        Id = id,
                        CountryId = countryId,
                        Name = parts[2].Trim(),
                        PopulationK = populationK
                    });
                }

                Cities.AddRange(cities);
                SaveChanges();
            }
        }
    }
}
