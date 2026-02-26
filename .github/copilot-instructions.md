# Football Stats API - AI Agent Workspace Instructions

## Code Style & C# Features

This project uses **C# 12** and **.NET 8** with modern patterns:

- **Primary constructors**: Used throughout for dependency injection (e.g., [FootballStatsContext](JT.FootballStats.Data/Context/FootballStatsContext.cs), [Repository<T>](JT.FootballStats.Data/Repositories/Repository.cs))
- **Collection expressions**: Initialize collections with `= []` instead of `new List<>()`
- **Nullable reference types**: Enabled in all projects (`<Nullable>enable</Nullable>`)
- **Required properties**: Domain models use `required` keyword (e.g., [League.cs](JT.FootballStats.Core/Models/League.cs), [Team.cs](JT.FootballStats.Core/Models/Team.cs))
- **Global usings**: Each project has [GlobalUsings.cs](JT.FootballStats.API/GlobalUsings.cs) - add common namespaces here
- **Implicit usings**: Enabled via `<ImplicitUsings>enable</ImplicitUsings>`
- **Top-level statements**: [Program.cs](JT.FootballStats.API/Program.cs) uses minimal hosting model
- **Minimal APIs**: Endpoints defined with `app.MapGet/MapPost` using route handlers

## Project Architecture

```
Core/           - Domain models, DTOs, config (no dependencies)
Data/           - EF Core context, repositories, services (→ Core)
Ingestion/      - External API clients (→ Core)
API/            - Web API entry point (→ Core, Data, Ingestion)
Tests/          - Integration tests (→ Core, Ingestion)
```

**Dependency Flow**: API → Data → Core ← Ingestion ← Tests

**Patterns Used**:
- **Repository Pattern**: Generic `IRepository<T>` in [Repositories/Repository.cs](JT.FootballStats.Data/Repositories/Repository.cs)
- **Unit of Work**: `IUnitOfWork` in [Repositories/UnitOfWork.cs](JT.FootballStats.Data/Repositories/UnitOfWork.cs) coordinates repositories
- **Service Layer**: Business logic in [Services/StandingsService.cs](JT.FootballStats.Data/Services/StandingsService.cs)
- **Options Pattern**: Configuration via `IOptions<T>` (see [ApiFootballConfig](JT.FootballStats.Core/Config/ApiFootballConfig.cs))
- **Dependency Injection**: Primary constructors for all services

## Build & Test Commands

```bash
# Build solution
dotnet build JT.FootballStats.sln

# Run tests
dotnet test JT.FootballStats.Tests/JT.FootballStats.Tests.csproj

# Run API
dotnet run --project JT.FootballStats.API/JT.FootballStats.API.csproj

# Create migration
dotnet ef migrations add MigrationName --project JT.FootballStats.Data --startup-project JT.FootballStats.API
```

## Project Conventions

### DTO Naming
- **External API responses**: Prefix with `Api` (e.g., [ApiStandingsResponse](JT.FootballStats.Core/DTOs/ApiStandingsResponse.cs), `ApiLeague`, `ApiTeam`)
- **API response DTOs**: Suffix with `Response` (e.g., [LeagueResponse](JT.FootballStats.Core/DTOs/LeagueResponse.cs), `StandingResponse`, `TeamResponse`)
- **Nested DTOs**: Group related DTOs in same file (see [ApiStandingsResponse.cs](JT.FootballStats.Core/DTOs/ApiStandingsResponse.cs))
- **JSON mapping**: Use `[JsonPropertyName]` attributes for external APIs

### Database Entity Configuration
In [FootballStatsContext.cs](JT.FootballStats.Data/Context/FootballStatsContext.cs):
- **Explicit configuration**: Use `OnModelCreating` for all entities
- **Required properties**: Mark with `.Property(e => e.Name).IsRequired()`
- **Cascade deletes**: Configure with `.OnDelete(DeleteBehavior.Cascade)`
- **Indexes**: Add on foreign keys using `.HasIndex(e => e.LeagueId)`
- **Default values**: Use `.HasDefaultValueSql("CURRENT_TIMESTAMP")` for timestamps

### External Entity IDs
For entities synced from external APIs ([League](JT.FootballStats.Core/Models/League.cs), [Team](JT.FootballStats.Core/Models/Team.cs)):
- Use **external ID as primary key**: `public required int Id { get; set; }`
- Configure with **ValueGeneratedNever**: `entity.Property(e => e.Id).ValueGeneratedNever()`
- **Upsert pattern**: Check existence, then add/update (see [StandingsService](JT.FootballStats.Data/Services/StandingsService.cs))

### Navigation Properties
- **Explicit foreign keys**: Define both FK property and navigation (e.g., `LeagueId` + `League`)
- **ForeignKey attribute**: Use `[ForeignKey(nameof(LeagueId))]` on navigation properties
- **Bidirectional**: Configure both sides (e.g., `League.Standings` ↔ `Standing.League`)
- **One-to-many without inverse**: Use `.WithMany()` without property (see [Standing.Team](JT.FootballStats.Core/Models/Standing.cs))

### HTTP Client Configuration
In [ApiFootballClient](JT.FootballStats.Ingestion/Clients/ApiFootballClient.cs):
- **Primary constructor**: Inject `HttpClient` and `IOptions<TConfig>`
- **BaseAddress**: Set in constructor, not in DI registration
- **Headers**: Add API keys to `DefaultRequestHeaders` in constructor
- **Registration**: Use `AddHttpClient<TClient>()` in [Program.cs](JT.FootballStats.API/Program.cs)

## Integration Points

- **External API**: API-Football (v3.football.api-sports.io)
- **Database**: SQLite via Entity Framework Core 8.0.22
- **Configuration**: Options pattern with section binding (`builder.Configuration.GetSection("ApiFootball")`)
- **JSON**: System.Text.Json with `PropertyNameCaseInsensitive` for deserialization

## Security & Secrets

- **API keys**: Store in **User Secrets**, never in appsettings.json
- **User Secrets ID**: Set in .csproj files (both [API](JT.FootballStats.API/JT.FootballStats.API.csproj) and [Tests](JT.FootballStats.Tests/JT.FootballStats.Tests.csproj))
- **Configuration structure**:
  ```json
  {
    "ApiFootball": {
      "ApiKey": "your-key-here"
    }
  }
  ```
- **Setup command**: `dotnet user-secrets set "ApiFootball:ApiKey" "your-key" --project JT.FootballStats.API`

## Testing

- **Framework**: xUnit with `IClassFixture<T>` for integration tests
- **Fixture pattern**: [IngestionTestFixture](JT.FootballStats.Tests/Ingestion/IngestionTestFixture.cs) sets up DI container with User Secrets
- **Primary constructor**: Tests receive fixture via primary constructor (see [ApiFootballClientIntegrationTests](JT.FootballStats.Tests/Ingestion/IntegrationTests/ApiFootballClientIntegrationTests.cs))
- **Naming**: Suffix integration test classes with `IntegrationTests`
