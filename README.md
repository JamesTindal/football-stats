# Football Stats API

A .NET 8 Web API for tracking and managing football league standings, synced from the API-Football external service.

## Features

- Sync Premier League standings from API-Football
- Store league, team, and standings data in SQLite database
- RESTful API endpoints for querying standings and league information
- Entity Framework Core with Repository and Unit of Work patterns

## Prerequisites

- [.NET 8 SDK](https://dotnet.microsoft.com/download/dotnet/8.0)
- API-Football API key (get one at [api-football.com](https://www.api-football.com/))

## Getting Started

### 1. Clone the repository

```bash
git clone <repository-url>
cd football-stats
```

### 2. Configure User Secrets

Store your API-Football API key securely using User Secrets:

```bash
dotnet user-secrets set "ApiFootball:ApiKey" "your-api-key-here" --project JT.FootballStats.API/JT.FootballStats.API.csproj
```

For running tests:

```bash
dotnet user-secrets set "ApiFootball:ApiKey" "your-api-key-here" --project JT.FootballStats.Tests/JT.FootballStats.Tests.csproj
```

### 3. Build the solution

```bash
dotnet build JT.FootballStats.sln
```

### 4. Run the API

```bash
dotnet run --project JT.FootballStats.API/JT.FootballStats.API.csproj
```

The API will be available at `https://localhost:5001` (or the port shown in the console).

### 5. Access Swagger UI

Navigate to `https://localhost:5001/swagger` to explore the API endpoints interactively.

## API Endpoints

### Sync Standings

```http
POST /api/standings/sync
```

Fetches the latest Premier League standings from API-Football and saves them to the database.

### Get All Standings

```http
GET /api/standings
```

Returns all standings with team and league information.

### Get League by ID

```http
GET /api/leagues/{id}
```

Returns a specific league with its standings.

**Example**: `GET /api/leagues/39` (Premier League)

## Project Structure

```
JT.FootballStats.sln
├── JT.FootballStats.API/          # Web API entry point
├── JT.FootballStats.Core/         # Domain models, DTOs, config
├── JT.FootballStats.Data/         # EF Core context, repositories, services
├── JT.FootballStats.Ingestion/    # External API clients
└── JT.FootballStats.Tests/        # Integration tests
```

## Technologies

- **.NET 8** with C# 12
- **ASP.NET Core** Minimal APIs
- **Entity Framework Core 8** with SQLite
- **System.Text.Json** for JSON serialization
- **xUnit** for testing
- **Swagger/OpenAPI** for API documentation

## Development

### Running Tests

```bash
dotnet test JT.FootballStats.Tests/JT.FootballStats.Tests.csproj
```

### Creating Database Migrations

```bash
dotnet ef migrations add MigrationName --project JT.FootballStats.Data --startup-project JT.FootballStats.API
```

### Applying Migrations

Migrations are automatically applied when the API starts. The SQLite database (`footballstats.db`) will be created in the API project directory.

## Architecture Patterns

- **Repository Pattern**: Generic `IRepository<T>` for data access
- **Unit of Work**: `IUnitOfWork` coordinates multiple repositories
- **Options Pattern**: Configuration via `IOptions<T>`
- **Dependency Injection**: Primary constructors throughout
- **Primary Keys**: External entity IDs (League, Team) used directly as primary keys

## Code Conventions

See [.github/copilot-instructions.md](.github/copilot-instructions.md) for detailed coding guidelines and conventions used in this project.