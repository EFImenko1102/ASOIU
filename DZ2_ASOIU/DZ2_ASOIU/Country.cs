namespace DZ2_ASOIU;

/// <summary>
/// Страна (справочная таблица, сторона "один").
/// </summary>
class Country
{
    /// <summary>Идентификатор страны.</summary>
    public int Id { get; set; }

    /// <summary>Название страны.</summary>
    public string Name { get; set; }

    /// <summary>Конструктор с параметрами.</summary>
    public Country(int id, string name)
    {
        Id = id;
        Name = name;
    }

    /// <summary>Конструктор по умолчанию.</summary>
    public Country() : this(0, "") { }

    /// <summary>Возвращает строковое представление страны.</summary>
    public override string ToString() => $"[{Id}] {Name}";
}
