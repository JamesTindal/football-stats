namespace JT.FootballStats.Data.Context;

public class FootballStatsContext(DbContextOptions<FootballStatsContext> options) : DbContext(options)
{
    public DbSet<League> Leagues { get; set; }
    public DbSet<Team> Teams { get; set; }
    public DbSet<Standing> Standings { get; set; }
}
