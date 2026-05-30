# Домашнее задание 2

## Вариант

Вариант 9: справочник `Страны`, основная таблица `Города`, числовое поле `population_k` - население в тысячах человек.

## Модель данных

Таблица `country`:

| Поле | Тип | Назначение |
| --- | --- | --- |
| `country_id` | INTEGER | первичный ключ |
| `country_name` | TEXT | название страны |

Таблица `city`:

| Поле | Тип | Назначение |
| --- | --- | --- |
| `city_id` | INTEGER | первичный ключ |
| `country_id` | INTEGER | внешний ключ на `country.country_id` |
| `city_name` | TEXT | название города |
| `population_k` | INTEGER | население, тыс. человек, ограничение `>= 0` |

## Реализованные файлы

- `DZ2_ASOIU/Country.cs` - класс справочника.
- `DZ2_ASOIU/City.cs` - класс основной таблицы с валидацией `PopulationK`.
- `DZ2_ASOIU/DatabaseManager.cs` - работа с SQLite, CSV, CRUD и SQL-запросами для отчетов.
- `DZ2_ASOIU/ReportBuilder.cs` - Fluent Interface для построения отчетов.
- `DZ2_ASOIU/Program.cs` - интерактивное консольное меню.
- `DZ2_ASOIU/country.csv` - тестовые страны.
- `DZ2_ASOIU/city.csv` - тестовые города.
- `DZ2_ASOIU/model_chen.puml` - модель данных в нотации Чена.
- `DZ2_ASOIU/model_ie.puml` - Entity Relationship Diagram.
- `DZ2_ASOIU/reportbuilder_activity.puml` - Activity Diagram для ReportBuilder.

## CSV-данные

Создано 4 страны и 12 городов, по 3 города на каждую страну.

## Обязательные отчеты

1. Полный список городов с названиями стран, `JOIN`, сортировка по названию города.
2. Количество городов в каждой стране, `GROUP BY` и `COUNT(*)`.
3. Среднее население городов по странам, `GROUP BY`, `AVG(...)`, сортировка по среднему населению.

## Дополнительные возможности

Добавлены:

- `Numbered()` в `ReportBuilder` - нумерация строк отчета.
- `Footer(string label)` в `ReportBuilder` - итоговая строка.
- `SaveToFile(string path)` в `ReportBuilder` - сохранение отчета в файл.
- пункт меню "Фильтр по стране".
- экспорт таблиц в CSV.

## Проверка

Проект собирается командой:

```bash
dotnet build DZ2_ASOIU.sln
```

В текущей среде установлен только .NET SDK/runtime 10, поэтому запуск `net8.0` проверялся так:

```bash
DOTNET_ROLL_FORWARD=Major dotnet run --project DZ2_ASOIU/DZ2_ASOIU.csproj
```

Результат сборки: `Build succeeded`, 0 предупреждений, 0 ошибок.
