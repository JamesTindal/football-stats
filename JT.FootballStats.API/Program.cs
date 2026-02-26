using System.Text.Json.Serialization;
using JT.FootballStats.Core.DTOs;
using JT.FootballStats.Data.Context;
using JT.FootballStats.Data.Repositories;
using JT.FootballStats.Data.Services;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

builder.Services.ConfigureHttpJsonOptions(options =>
{
    options.SerializerOptions.ReferenceHandler = ReferenceHandler.IgnoreCycles;
    options.SerializerOptions.DefaultIgnoreCondition = JsonIgnoreCondition.WhenWritingNull;
});

builder.Services.AddHttpClient<ApiFootballClient>();

builder.Services.Configure<ApiFootballConfig>(
    builder.Configuration.GetSection("ApiFootball")
);

builder.Services.AddDbContext<FootballStatsContext>(options =>
    options.UseSqlite("Data Source=footballstats.db")
);

builder.Services.AddScoped(typeof(IRepository<>), typeof(Repository<>));
builder.Services.AddScoped<IUnitOfWork, UnitOfWork>();

builder.Services.AddScoped<IStandingsService, StandingsService>();

var app = builder.Build();

using (var scope = app.Services.CreateScope())
{
    var dbContext = scope.ServiceProvider.GetRequiredService<FootballStatsContext>();
    await dbContext.Database.MigrateAsync();
}

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.MapPost("/api/standings/sync", async (ApiFootballClient apiClient, IStandingsService standingsService) =>
{
    var apiResponse = await apiClient.GetCurrentPremierLeagueStandingsAsync();
    
    if (apiResponse == null)
        return Results.BadRequest("Failed to fetch standings from API");
    
    await standingsService.SaveStandingsAsync(apiResponse);
    
    return Results.Ok("Standings synchronized successfully");
})
.WithName("SyncStandings")
.WithOpenApi();

app.MapGet("/api/standings", async (FootballStatsContext context) =>
{
    var standings = await context.Standings
        .Include(s => s.Team)
        .Include(s => s.League)
        .ToListAsync();
    
    var response = standings.Select(s => new StandingResponse
    {
        Id = s.Id,
        Rank = s.Rank,
        TeamId = s.TeamId,
        TeamName = s.Team.Name,
        Points = s.Points,
        Played = s.Played,
        Won = s.Won,
        Drawn = s.Drawn,
        Lost = s.Lost,
        GoalsFor = s.GoalsFor,
        GoalsAgainst = s.GoalsAgainst,
        UpdatedAt = s.UpdatedAt
    });
    
    return Results.Ok(response);
})
.WithName("GetStandings")
.WithOpenApi();

app.MapGet("/api/leagues/{id}", async (int id, FootballStatsContext context) =>
{
    var league = await context.Leagues.FindAsync(id);
    
    if (league == null)
        return Results.NotFound($"League with ID {id} not found");
    
    var standings = await context.Standings
        .Include(s => s.Team)
        .Where(s => s.LeagueId == id)
        .ToListAsync();
    
    var response = new LeagueResponse
    {
        Id = league.Id,
        Name = league.Name,
        Season = league.Season,
        Standings = standings.Select(s => new StandingResponse
        {
            Id = s.Id,
            Rank = s.Rank,
            TeamId = s.TeamId,
            TeamName = s.Team.Name,
            Points = s.Points,
            Played = s.Played,
            Won = s.Won,
            Drawn = s.Drawn,
            Lost = s.Lost,
            GoalsFor = s.GoalsFor,
            GoalsAgainst = s.GoalsAgainst,
            UpdatedAt = s.UpdatedAt
        }).OrderBy(s => s.Rank).ToList()
    };
    
    return Results.Ok(response);
})
.WithName("GetLeagueWithStandings")
.WithOpenApi();

app.MapGet("/api/teams", async (IUnitOfWork unitOfWork) =>
{
    var teams = await unitOfWork.Teams.GetAllAsync();
    
    var response = teams.Select(t => new TeamResponse
    {
        Id = t.Id,
        Name = t.Name
    });
    
    return Results.Ok(response);
})
.WithName("GetTeams")
.WithOpenApi();

await app.RunAsync();