namespace JT.FootballStats.Core.DTOs;

public class LeagueResponse
{
    public int Id { get; set; }
    public string Name { get; set; } = string.Empty;
    public int Season { get; set; }
    public List<StandingResponse> Standings { get; set; } = [];
}

public class StandingResponse
{
    public int Id { get; set; }
    public int Rank { get; set; }
    public int TeamId { get; set; }
    public string TeamName { get; set; } = string.Empty;
    public int Points { get; set; }
    public int Played { get; set; }
    public int Won { get; set; }
    public int Drawn { get; set; }
    public int Lost { get; set; }
    public int GoalsFor { get; set; }
    public int GoalsAgainst { get; set; }
    public int GoalDifference => GoalsFor - GoalsAgainst;
    public DateTime UpdatedAt { get; set; }
}

public class TeamResponse
{
    public int Id { get; set; }
    public string Name { get; set; } = string.Empty;
}
