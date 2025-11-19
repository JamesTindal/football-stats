namespace JT.FootballStats.Core.Models;

public class League
{
    public required int Id { get; set; }

    public required string Name { get; set; }

    public required List<Team> Teams { get; set; }
    
}