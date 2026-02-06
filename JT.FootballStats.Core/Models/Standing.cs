using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace JT.FootballStats.Core.Models;

public class Standing
{
    [Key]
    public int Id { get; set; }

    public int LeagueId { get; set; }

    [ForeignKey(nameof(LeagueId))]
    public required League League { get; set; }

    public int TeamId { get; set; }

    [ForeignKey(nameof(TeamId))]
    public required Team Team { get; set; }

    [Range(1, 20)]
    public required int Rank { get; set; }

    [Range(0, 114)]
    public required int Points { get; set; }

    [Range(0, 38)]
    public required int Played { get; set; }

    [Range(0, 38)]
    public required int Won { get; set; }

    [Range(0, 38)]
    public required int Drawn { get; set; }

    [Range(0, 38)]
    public required int Lost { get; set; }

    public required int GoalsFor { get; set; }

    public required int GoalsAgainst { get; set; }
}
