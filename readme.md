# Pierwsze uruchomienie projektu
```
# Przywrócenie zależności NuGet
dotnet restore
# Instalacja globalnego  narzędzia dotnet w razie jego rbaku
dotnet tool install --global dotnet-ef
# Utworzenie naszej bazy danych SQLite
dotnet ef database update
```
# Dane do logowania na testowe konto administratorskie
Email: admin@cms.pl

Hasło: Admin123!

# Łancuch połączeń
Łańcuch połączeń możemy ustawić w pliku ```appsettings.json```
Domyślna wartość to:
``"DefaultConnection": "Data Source=blogsy.db"``. Data source określa nazwe pliku bazy danych SQLite który zostanie utworzony przy migracji.
