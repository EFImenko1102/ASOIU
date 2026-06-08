# Семинар 3

Проект состоит из двух частей:
1. `seminar_3_db` — работа с БД SQLite (создание БД, загрузка данных из CSV, демонстрация выборки, проекции, соединения и группировки).
2. `tdb` — консольный прототип СУБД для обработки CSV-файлов через стандартные потоки ввода-вывода (stdin/stdout).

## Сборка проекта

Сборка всего решения:
```bash
dotnet build
```

## Запуск seminar_3_db (SQLite)

Программа создаст файл БД `developers.db`, загрузит данные и выполнит демонстрационные запросы:
```bash
dotnet run --project seminar_3_db
```

## Запуск tdb (Прототип СУБД)

Программа ожидает данные на стандартный вход (stdin) для операций выборки, проекции и группировки, либо считывает файлы по именам для операции соединения.

### Примеры запуска

**Выборка (where):**
```bash
dotnet tdb/bin/Debug/net10.0/tdb.dll where dep_id 2 < dev.csv
```

**Проекция (projection):**
```bash
dotnet tdb/bin/Debug/net10.0/tdb.dll projection dev_name < dev.csv
```

**Соединение таблиц (join):**
```bash
dotnet tdb/bin/Debug/net10.0/tdb.dll join dev dep dep_id dep_id
```

**Группировка по среднему (group_avg):**
```bash
dotnet tdb/bin/Debug/net10.0/tdb.dll group_avg dep_id dev_commits < dev.csv
```

**Конвейер (выборка по условию + получение конкретной колонки):**
```bash
dotnet tdb/bin/Debug/net10.0/tdb.dll where dep_id 2 < dev.csv | dotnet tdb/bin/Debug/net10.0/tdb.dll projection dev_name
```
