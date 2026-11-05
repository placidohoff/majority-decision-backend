# Majority Decision API

ASP.NET Core (.NET 9) Web API with Entity Framework Core (SQL Server) for the Majority Decision app.

## Prerequisites
- .NET 9 SDK
- SQL Server LocalDB (ships with Visual Studio). Change `ConnectionStrings:DefaultConnection` in `appsettings.json` to use another SQL Server instance.

## Getting started
```powershell
dotnet tool restore                 # installs the pinned dotnet-ef tool
dotnet tool run dotnet-ef database update   # creates/updates the database
dotnet run --launch-profile http    # http://localhost:5148
```
OpenAPI document (Development only): `http://localhost:5148/openapi/v1.json`

## Migrations
```powershell
dotnet tool run dotnet-ef migrations add <Name> -o Data/Migrations
dotnet tool run dotnet-ef database update
dotnet tool run dotnet-ef migrations remove   # undo the last, unapplied migration
```

## Layout
- `Models/` – entities (`Fighter`, `Fight`, `Scorecard`, `RoundScore`, `WeightClass`)
- `Data/AppDbContext.cs` – EF Core model configuration
- `Data/Migrations/` – EF Core migrations
- `Controllers/` – API controllers

## CORS
Allowed origins are configured under `Cors:AllowedOrigins` (defaults to the Next.js dev server, `http://localhost:3000`).
