# Server_HTTP

Prosty serwer HTTP napisany w C#/.NET Core. Przyjmuje połączenia TCP na porcie `8081` i udostępnia pliki statyczne z katalogu `Server_HTTP/www` (np. `index.html`). Obsługuje klientów za pomocą asynchronicznych socketów.

## Uruchomienie

Wymagany jest .NET Core 3.0 SDK. Z katalogu głównego projektu uruchom:

```bash
dotnet run --project Server_HTTP/Server_HTTP.csproj
```

Serwer będzie dostępny pod adresem [http://localhost:8081](http://localhost:8081).
