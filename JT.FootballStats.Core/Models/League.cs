using System.ComponentModel.DataAnnotations;

namespace JT.FootballStats.Core.Models;

public class League
{
    [Key]
    public required int Id { get; set; }

    public required string Name { get; set; }

    public required int Season { get; set; }

    public List<Standing> Standings { get; set; } = [];
}