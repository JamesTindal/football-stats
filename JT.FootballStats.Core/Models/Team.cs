using System.ComponentModel.DataAnnotations;

namespace JT.FootballStats.Core.Models;

public class Team
{
    [Key]
    public required int Id { get; set; }

    public required string Name { get; set; }
    
}
