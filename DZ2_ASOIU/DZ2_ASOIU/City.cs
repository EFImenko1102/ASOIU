namespace DZ2_ASOIU;

/// <summary>
/// Город (основная таблица, сторона "много").
/// </summary>
class City
{
    private int _populationK;

    /// <summary>Идентификатор города.</summary>
    public int Id { get; set; }

    /// <summary>Идентификатор страны (внешний ключ).</summary>
    public int CountryId { get; set; }

    /// <summary>Название города.</summary>
    public string Name { get; set; }

    /// <summary>Население города в тысячах человек. Не может быть отрицательным.</summary>
    public int PopulationK
    {
        get => _populationK;
        set
        {
            if (value < 0)
                throw new ArgumentException("Население не может быть отрицательным");

            _populationK = value;
        }
    }

    /// <summary>Конструктор с параметрами.</summary>
    public City(int id, int countryId, string name, int populationK)
    {
        Id = id;
        CountryId = countryId;
        Name = name;
        PopulationK = populationK;
    }

    /// <summary>Конструктор по умолчанию.</summary>
    public City() : this(0, 0, "", 0) { }

    /// <summary>Возвращает строковое представление города.</summary>
    public override string ToString()
        => $"[{Id}] {Name}, страна #{CountryId}, население: {PopulationK} тыс. чел.";
}
