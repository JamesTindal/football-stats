namespace JT.FootballStats.Data.Repositories;

public interface IUnitOfWork : IDisposable
{
    IRepository<League> Leagues { get; }
    IRepository<Team> Teams { get; }
    IRepository<Standing> Standings { get; }
    Task<int> SaveChangesAsync();
}
