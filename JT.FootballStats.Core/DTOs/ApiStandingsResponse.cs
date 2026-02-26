using System.Text.Json.Serialization;

namespace JT.FootballStats.Core.DTOs;

public class ApiStandingsResponse
{
    [JsonPropertyName("response")]
    public List<ApiLeagueResponse> Response { get; set; } = [];
}

public class ApiLeagueResponse
{
    [JsonPropertyName("league")]
    public ApiLeague League { get; set; } = new();
}

public class ApiLeague
{
    [JsonPropertyName("id")]
    public int Id { get; set; }

    [JsonPropertyName("name")]
    public string Name { get; set; } = string.Empty;

    [JsonPropertyName("season")]
    public int Season { get; set; }

    [JsonPropertyName("standings")]
    public List<List<ApiStandingEntry>> Standings { get; set; } = [];
}

public class ApiStandingEntry
{
    [JsonPropertyName("rank")]
    public int Rank { get; set; }

    [JsonPropertyName("team")]
    public ApiTeam Team { get; set; } = new();

    [JsonPropertyName("points")]
    public int Points { get; set; }

    [JsonPropertyName("all")]
    public ApiMatchStats All { get; set; } = new();

    [JsonPropertyName("update")]
    public DateTime Update { get; set; }
}

public class ApiTeam
{
    [JsonPropertyName("id")]
    public int Id { get; set; }

    [JsonPropertyName("name")]
    public string Name { get; set; } = string.Empty;
}

public class ApiMatchStats
{
    [JsonPropertyName("played")]
    public int Played { get; set; }

    [JsonPropertyName("win")]
    public int Win { get; set; }

    [JsonPropertyName("draw")]
    public int Draw { get; set; }

    [JsonPropertyName("lose")]
    public int Lose { get; set; }

    [JsonPropertyName("goals")]
    public ApiGoals Goals { get; set; } = new();
}

public class ApiGoals
{
    [JsonPropertyName("for")]
    public int For { get; set; }

    [JsonPropertyName("against")]
    public int Against { get; set; }
}
